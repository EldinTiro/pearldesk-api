using ErrorOr;
using MediatR;

namespace PearlDesk.Tenants.Application.Commands;

public record CreateTenantCommand(
    string Slug,
    string Name,
    string Plan,
    string OwnerEmail,
    string OwnerFirstName,
    string OwnerLastName,
    string? LogoBase64 = null) : IRequest<ErrorOr<TenantCreatedResponse>>;
