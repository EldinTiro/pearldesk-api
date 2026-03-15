# PearlDesk — Implementation Plan

> Multi-tenant dental practice management SaaS  
> Last updated: 2026-03-14

---

## Table of Contents

1. [Overview](#overview)
2. [Tech Stack](#tech-stack)
3. [Authentication](#authentication)
4. [Cloud Deployment](#cloud-deployment)
5. [Solution Structure](#solution-structure)
6. [Multi-Tenancy](#multi-tenancy)
7. [Backend Architecture](#backend-architecture)
8. [Feature Modules](#feature-modules)
9. [API Design](#api-design)
10. [Frontend Architecture](#frontend-architecture)
11. [Implementation Phases](#implementation-phases)
12. [Future Features (Backlog)](#future-features-backlog--lowest-priority)
13. [Key Technical Decisions](#key-technical-decisions)

---

## Overview

PearlDesk is a multi-tenant, cloud-hosted dental practice management application. Each dental clinic is a tenant that gets its own subdomain (e.g. `clinicname.pearldesk.com`). All tenants share a single PostgreSQL database, isolated at the row level via `tenant_id` on every table.

**Core characteristics:**
- Multi-tenant SaaS (row-level isolation, subdomain-based resolution)
- .NET 9 Modular Monolith — Clean Architecture + CQRS
- React 19 + TypeScript frontend
- PostgreSQL 17 database
- Containerised deployment (Docker) to AWS

---

## Tech Stack

### Backend

| Concern | Library / Tool |
|---|---|
| Framework | .NET 9 — ASP.NET Core |
| API layer | FastEndpoints |
| CQRS dispatching | MediatR |
| ORM | Entity Framework Core 9 + Npgsql |
| Multi-tenancy | Finbuckle.MultiTenant |
| Validation | FluentValidation (integrated with FastEndpoints) |
| Error handling | ErrorOr (Result pattern) |
| Auth | ASP.NET Core Identity + JWT Bearer + Refresh Tokens |
| Background jobs | Hangfire + PostgreSQL storage |
| Logging | Serilog + structured sinks |
| Testing | xUnit + NSubstitute + FluentAssertions |
| Payments | Stripe.NET |

### Frontend

| Concern | Library / Tool |
|---|---|
| Framework | React 19 + TypeScript + Vite |
| Server state | TanStack Query (React Query) |
| Routing | React Router v7 |
| UI components | Shadcn/ui + Tailwind CSS |
| Forms & validation | React Hook Form + Zod |
| Calendar | FullCalendar |
| Charts | Recharts |

### Infrastructure

| Concern | Tool |
|---|---|
| Local dev | Docker + Docker Compose |
| Database | PostgreSQL 17 |
| Caching | Redis |
| IaC | Terraform |

---

## Authentication

**Choice: ASP.NET Core Identity + JWT Bearer + Refresh Tokens**

Stays entirely within the .NET ecosystem, no external identity server dependency, full control, and multi-tenant friendly via Finbuckle.

### Roles (per tenant)

| Role | Scope |
|---|---|
| `ClinicAdmin` | Tenant-scoped admin |
| `Dentist` | Clinical staff |
| `DentalAssistant` | Clinical support |
| `Receptionist` | Front desk |
| `Patient` | Patient portal access |
| `SuperAdmin` | Platform-wide (outside tenant scope) |

---

## Cloud Deployment

Deployment uses Docker containers, keeping the application cloud-portable.

### AWS

| Resource | Service |
|---|---|
| Application containers | ECS Fargate |
| PostgreSQL | RDS for PostgreSQL |
| Redis | ElastiCache |
| Document storage | S3 |
| Email | SES |
| SMS | Twilio |
| DNS + subdomains | Route 53 |
| React frontend CDN | CloudFront + S3 |
| Monitoring & logs | CloudWatch + X-Ray |
| IaC | Terraform |

---

## Solution Structure

```
PearlDesk/
├── pearldesk-api/
│   ├── src/
│   │   ├── PearlDesk.API/                  # Entry point: FastEndpoints, middleware, DI wiring
│   │   ├── PearlDesk.Application/          # CQRS handlers, interfaces, pipeline behaviors
│   │   ├── PearlDesk.Domain/               # Entities, value objects, enums, typed errors
│   │   ├── PearlDesk.Infrastructure/       # EF Core, repositories, external service adapters
│   │   │
│   │   └── Modules/
│   │       ├── PearlDesk.Tenants/
│   │       ├── PearlDesk.Identity/
│   │       ├── PearlDesk.Staff/
│   │       ├── PearlDesk.Patients/
│   │       ├── PearlDesk.Appointments/
│   │       ├── PearlDesk.Treatments/
│   │       ├── PearlDesk.Billing/
│   │       ├── PearlDesk.Notifications/
│   │       ├── PearlDesk.Documents/
│   │       └── PearlDesk.Reporting/
│   │
│   └── tests/
│       ├── PearlDesk.Patients.Tests/
│       ├── PearlDesk.Appointments.Tests/
│       └── ... (one test project per module)
│
├── pearldesk-web/                               # React application
│   └── src/
│       ├── features/                       # Feature-sliced: patients/, appointments/, etc.
│       ├── components/                     # Shared UI components
│       ├── lib/                            # API client, auth helpers, utilities
│       └── routes/                         # React Router route definitions
│
├── infrastructure/
│   └── aws/                                # Terraform modules for AWS
│
├── documentation/                          # All implementation plans and ADRs
│   ├── implementation-plan.md              # This file
│   └── data-model.md                       # Full entity & table reference (all modules)
├── docker-compose.yml                      # Local dev: API + React + Postgres + Redis
└── .github/workflows/                      # CI/CD pipelines
```

---

## Multi-Tenancy

### Strategy: Row-level isolation via Finbuckle.MultiTenant

1. Every database table includes a `tenant_id` column (GUID).
2. `Finbuckle.MultiTenant` resolves the current tenant from the request subdomain.
3. A custom `TenantDbContext` base class registers EF Core Global Query Filters on all tenant-scoped entities, automatically appending `WHERE tenant_id = @current_tenant_id` to every query.
4. **Soft deletes** — All tenant-scoped entities implement a `ISoftDeletable` interface (`IsDeleted`, `DeletedAt`). The Global Query Filter also appends `WHERE is_deleted = false`, ensuring soft-deleted records are invisible to all queries by default. Hard deletes are never performed on clinical or financial data.
5. Tenant creation triggers a Hangfire seeding job (default roles, settings, admin user).

**Subdomain pattern:** `{clinic-slug}.pearldesk.com`

### Soft Delete Interface

```csharp
public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
}
```

All entities that implement `ISoftDeletable` automatically receive both filters in `TenantDbContext.OnModelCreating`:

```csharp
builder.HasQueryFilter(e =>
    EF.Property<Guid>(e, "TenantId") == _currentTenantId &&
    !EF.Property<bool>(e, "IsDeleted"));
```

A `SoftDeleteRepository` base class exposes a `SoftDeleteAsync` method instead of `Remove`, setting `IsDeleted = true` and `DeletedAt = UtcNow`.

---

## Backend Architecture

### Clean Architecture Layers

```
PearlDesk.Domain/
│   Entities/            ← Pure domain objects (no EF, no MediatR)
│   ValueObjects/        ← e.g. ToothNumber, Money, PhoneNumber
│   Enums/
│   Errors/              ← Typed error definitions per aggregate
│                          e.g. PatientErrors.NotFound, AppointmentErrors.Conflict

PearlDesk.Application/
│   Common/
│     Behaviors/         ← MediatR pipeline: ValidationBehavior, LoggingBehavior, PerformanceBehavior
│     Interfaces/        ← IPatientRepository, IEmailSender, IFileStorage, etc.
│   Features/
│     Patients/
│       Commands/        ← CreatePatient, UpdatePatient, DeletePatient, ...
│       Queries/         ← GetPatientById, ListPatients, ...
│     Appointments/
│       Commands/
│       Queries/
│     ... (one folder per feature/module)

PearlDesk.Infrastructure/
│   Persistence/
│     DbContext/         ← TenantDbContext (EF Core + Finbuckle integration)
│     Repositories/      ← EF Core implementations of application interfaces
│     Configurations/    ← IEntityTypeConfiguration per entity
│     Migrations/
│   ExternalServices/
│     Email/             ← SES / SendGrid adapter
│     Sms/               ← Twilio adapter
│     Storage/           ← S3 adapter
│   BackgroundJobs/      ← Hangfire job definitions

PearlDesk.API/
│   Endpoints/           ← FastEndpoints endpoint classes (one class per operation)
│   Middleware/          ← Tenant resolution, auth, exception fallback
│   Mapping/             ← Request → Command/Query mappers
│   DependencyInjection/ ← Module registration extensions
```

### MediatR Pipeline Behaviors (in order)

1. **`LoggingBehavior`** — logs command/query name, tenant ID, duration
2. **`ValidationBehavior`** — runs FluentValidation; short-circuits with `ErrorOr` validation errors before the handler executes
3. **`PerformanceBehavior`** — logs a warning if a handler exceeds a configurable threshold (e.g. 500ms)

### Error Handling — ErrorOr Result Pattern

Every MediatR handler returns `ErrorOr<T>`. Errors are typed and explicit; no exceptions used for flow control.

```csharp
// Application layer
public async Task<ErrorOr<AppointmentResponse>> Handle(BookAppointmentCommand command, CancellationToken ct)
{
    var patient = await _patients.GetByIdAsync(command.PatientId, ct);
    if (patient is null)
        return Error.NotFound("Patient.NotFound", "Patient does not exist.");

    return new AppointmentResponse(...);
}

// API layer — FastEndpoints maps result to HTTP
public override async Task HandleAsync(BookAppointmentRequest req, CancellationToken ct)
{
    var result = await _sender.Send(req.ToCommand());

    await result.MatchAsync(
        response => SendOkAsync(response, ct),
        errors   => SendErrorsAsync(errors, ct)
    );
}
```

### FastEndpoints Conventions

- One class per operation (e.g. `BookAppointmentEndpoint`, `GetPatientByIdEndpoint`)
- Endpoints grouped by feature using FastEndpoints' built-in `Group`
- API versioning via FastEndpoints versioning (no separate middleware)
- Validation via FluentValidation validators auto-scanned by FastEndpoints
- Authorization via `.Roles(...)` or `.Policies(...)` per endpoint

### Unit Testing

- **Target:** MediatR handlers (Application layer) + Domain objects
- **Tools:** xUnit + NSubstitute + FluentAssertions
- Repositories and external services mocked with NSubstitute
- Both success and error branches of `ErrorOr` results asserted
- One test project per module

---

## Feature Modules

### Tenants
- Clinic registration & onboarding wizard
- Subdomain provisioning
- Plan / subscription management
- Per-tenant configuration (name, logo, timezone, working hours)

### Identity
- Login, registration, password reset
- JWT + refresh token rotation
- 2FA (TOTP)
- Role & permission management
- Tenant-scoped user lists

### Staff
- Staff CRUD
- Role assignment
- Schedule & availability configuration
- Leave management

### Patients
- Patient CRUD
- Demographic data
- Medical history & allergies
- Dental insurance
- Emergency contacts
- Patient portal (read-only view of own records)

### Appointments
- Calendar (day / week / month view)
- Booking, rescheduling, cancellation
- Provider availability engine
- Buffer time between appointments
- Recurring appointments
- Cancellation policies
- Waiting list

### Treatments
- Treatment plans linked to patients
- CDT procedure code library
- Visual dental chart with per-tooth annotations
- SOAP clinical notes
- Treatment history timeline

### Billing
- Invoice generation from completed treatments
- Itemised line items
- Payment recording
- Stripe integration (card payments)
- Insurance claim tracking
- Payment history

### Notifications
- Email & SMS via pluggable providers (SES/SendGrid for email, Twilio for SMS)
- Template engine with per-tenant customisation
- Hangfire-scheduled appointment reminders (24h and 2h before appointment)
- Notification log

### Documents
- Secure file upload (X-rays, consent forms, images)
- Storage in S3 (AWS)
- Time-limited signed download URLs
- Per-patient document list

### Reporting
- Revenue by period
- Appointment volume & cancellation rates
- Patient demographics
- Staff utilisation
- Export to PDF and Excel

---

## API Design

- RESTful, versioned: `/api/v1/...`
- OpenAPI / Swagger documentation (FastEndpoints built-in)
- Global exception handler middleware returning RFC 9457 `ProblemDetails`
- Tenant validated on every request via middleware
- Rate limiting per tenant (ASP.NET Core built-in rate limiter)

---

## Frontend Architecture

- **React 19 + TypeScript + Vite**
- Feature-sliced folder structure mirroring backend modules
- TanStack Query for all server state (caching, invalidation, optimistic updates)
- React Router v7 for routing with lazy-loaded feature routes
- Shadcn/ui + Tailwind CSS for consistent, accessible UI
- React Hook Form + Zod for all form handling and client-side validation
- FullCalendar for the appointment scheduling views
- Recharts for reporting & analytics dashboards
- Subdomain-based tenant detection on app bootstrap

---

## Implementation Phases

### Phase 1 — Foundation (Weeks 1–3)
- Solution skeleton + Docker Compose local environment (see below)
- PostgreSQL migrations baseline + EF Core setup
- `ISoftDeletable` interface + Global Query Filter for soft deletes
- Finbuckle.MultiTenant integration + Global Query Filters
- ASP.NET Core Identity + JWT + refresh tokens
- Tenant registration & subdomain resolution
- CI/CD pipeline (GitHub Actions) — build, test, push image

#### Docker Compose — Local Development

The `docker-compose.yml` at the repository root provides a complete local environment:

```yaml
services:
  api:
    build:
      context: ./backend
      dockerfile: Dockerfile
    ports:
      - "5000:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=pearldesk;Username=pearldesk;Password=pearldesk
      - Redis__ConnectionString=redis:6379
    depends_on:
      postgres:
        condition: service_healthy
      redis:
        condition: service_healthy

  frontend:
    build:
      context: ./pearldesk-web
      dockerfile: Dockerfile
    ports:
      - "5173:5173"
    environment:
      - VITE_API_URL=http://localhost:5000

  postgres:
    image: postgres:17-alpine
    ports:
      - "5432:5432"
    environment:
      POSTGRES_DB: pearldesk
      POSTGRES_USER: pearldesk
      POSTGRES_PASSWORD: pearldesk
    volumes:
      - postgres_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U pearldesk"]
      interval: 5s
      timeout: 5s
      retries: 5

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    healthcheck:
      test: ["CMD", "redis-cli", "ping"]
      interval: 5s
      timeout: 5s
      retries: 5

  hangfire-dashboard:
    # Hangfire dashboard is served by the API — accessible at http://localhost:5000/hangfire
    # Restricted to SuperAdmin role in production

volumes:
  postgres_data:
  redis_data:
```

**Usage:**
```bash
docker compose up --build      # start all services
docker compose down -v         # stop and remove volumes
```

### Phase 2a — Core Backend Modules (Weeks 4–6)
- Staff module (profiles, roles, schedules)
- Patients module (full CRUD + medical history)
- Appointments module (calendar, booking engine, availability)
- Basic email notifications (appointment confirmation & reminders via SES)
- Unit tests for all new handlers

### Phase 2b — Frontend Scaffolding & Core UI (Weeks 7–8)
- Vite + React 19 + TypeScript project setup
- Shadcn/ui + Tailwind CSS configuration
- React Router v7 routing structure + lazy-loaded feature routes
- Subdomain-based tenant detection on app bootstrap
- Auth flows: login, logout, token refresh, role-based route guards
- App shell: sidebar navigation, header, layout per role
- First feature screens: Patients list/detail, Appointments calendar view
- TanStack Query setup + API client (Axios/fetch wrapper)

### Phase 3 — Clinical Features (Weeks 9–12)
- Treatment plans & visual dental charting
- CDT code library
- SOAP clinical notes
- Document uploads (X-rays, consent forms)

### Phase 4 — Business Features (Weeks 13–16)
- Billing & invoicing
- Stripe payment integration
- Insurance claim tracking
- SMS reminders (Twilio)
- Reporting module + PDF / Excel export

### Phase 5 — Production Readiness (Weeks 17–20)
- Terraform cloud infrastructure (AWS)
- Security audit (OWASP checklist)
- Performance testing + query optimisation
- End-to-end tests (Playwright)
- Production deployment + monitoring dashboards (CloudWatch + X-Ray)
- Onboarding documentation for clinic admins

---

## Future Features (Backlog — Lowest Priority)

These features are planned but intentionally deferred until the core product is stable. They should be revisited after Phase 5.

### F1 — Smart Appointment Reminders
Automated SMS reminders sent to patients before appointments.
- Twilio (primary) or MessageBird (fallback/alternative) as SMS provider
- Configurable reminder schedule per tenant (e.g. 24h and 2h before appointment)
- Delivery status tracking and retry logic via Hangfire
- Per-patient opt-out support
- Reminder templates customisable per tenant

### F2 — Interactive Dental Chart
A visual, clickable dental chart for dentists to record per-tooth clinical data.
- Click on any tooth (FDI or Universal numbering) to open a detail panel
- Per-tooth condition tracking: crown, implant, filling, extraction, bridge, root canal, etc.
- Full history of changes per tooth with timestamps
- Visual colour coding by condition type
- Integrates with Treatment Plans (F3) and SOAP clinical notes

### F3 — Treatment Plan Generator
Structured treatment planning workflow for dentists.
- Create multi-step treatment plans linked to a patient
- Each plan item references a CDT procedure code, tooth number(s), estimated cost, and priority
- Plan status workflow: Draft → Presented → Accepted / Declined → In Progress → Completed
- PDF export of the treatment plan for patient sign-off
- Link completed plan items to invoices in the Billing module

### F4 — Online Booking Portal
A public-facing booking page allowing patients to self-schedule appointments.
- Accessible via `{clinic-slug}.pearldesk.com/book` — no login required to browse availability
- Patients select service type, preferred dentist, and available time slot
- Booking confirmation sent via email and SMS
- Configurable booking rules per tenant (lead time, cancellation window, max future bookings)
- Optional patient account creation at time of booking
- Integrates with the provider availability engine in the Appointments module

---

## Key Technical Decisions

| Decision | Choice | Reason |
|---|---|---|
| Multi-tenancy strategy | Row-level (Finbuckle) | Cost-effective, simpler ops, strong isolation via EF Global Query Filters |
| Soft deletes | `ISoftDeletable` + Global Query Filter | Clinical/financial data must never be hard-deleted; invisible by default via EF filter |
| Architecture | Modular Monolith | Right-sized for SaaS MVP; modules can be extracted to microservices later |
| API layer | FastEndpoints | Minimal, performant, no reflection-heavy controller conventions |
| CQRS dispatching | MediatR | Industry standard, clean separation of commands and queries |
| Error handling | ErrorOr Result pattern | Explicit, typed errors; no exception-driven flow control |
| Domain events | None (for now) | Keeps complexity low; direct service calls between modules |
| Auth | ASP.NET Core Identity + JWT | No external dependency, full control, multi-tenant friendly |
| Background jobs | Hangfire + PostgreSQL | Reliable retry, no extra infrastructure |
| Deployment | Docker containers on AWS | ECS Fargate + RDS + CloudFront |
| IaC | Terraform | Cloud-portable, battle-tested for AWS |
| Testing | Unit tests (xUnit + NSubstitute) | Handler and domain logic covered; integration tests deferred |
