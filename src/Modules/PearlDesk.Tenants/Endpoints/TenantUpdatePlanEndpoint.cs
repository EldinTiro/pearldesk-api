using FastEndpoints;
using MediatR;
using DomainRoles = PearlDesk.Domain.Identity.Roles;
using PearlDesk.Tenants.Application;
using PearlDesk.Tenants.Application.Commands;

namespace PearlDesk.Tenants.Endpoints;

public class TenantUpdatePlanEndpoint(ISender sender) : Endpoint<UpdateTenantPlanRequest, TenantResponse>
{
    public override void Configure()
    {
        Put("/tenants/{id}/plan");
        Roles(DomainRoles.SuperAdmin);
        Version(1);
        Summary(s => s.Summary = "Update a tenant's subscription plan");
    }

    public override async Task HandleAsync(UpdateTenantPlanRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await sender.Send(new UpdateTenantPlanCommand(id, req.Plan, req.ExpiresAt), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendOkAsync(result.Value, ct);
    }
}

public record UpdateTenantPlanRequest(string Plan, DateTime? ExpiresAt);
