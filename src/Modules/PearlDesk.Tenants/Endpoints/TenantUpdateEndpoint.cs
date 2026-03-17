using FastEndpoints;
using MediatR;
using DomainRoles = PearlDesk.Domain.Identity.Roles;
using PearlDesk.Tenants.Application;
using PearlDesk.Tenants.Application.Commands;

namespace PearlDesk.Tenants.Endpoints;

public class TenantUpdateEndpoint(ISender sender) : Endpoint<UpdateTenantRequest, TenantResponse>
{
    public override void Configure()
    {
        Put("/tenants/{id}");
        Roles(DomainRoles.SuperAdmin);
        Version(1);
        Summary(s => s.Summary = "Update a tenant's name");
    }

    public override async Task HandleAsync(UpdateTenantRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await sender.Send(new UpdateTenantCommand(id, req.Name, req.LogoBase64), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendOkAsync(result.Value, ct);
    }
}

public record UpdateTenantRequest(string Name, string? LogoBase64 = null);
