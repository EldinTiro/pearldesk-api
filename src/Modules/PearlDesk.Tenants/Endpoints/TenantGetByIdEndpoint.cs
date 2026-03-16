using FastEndpoints;
using MediatR;
using DomainRoles = PearlDesk.Domain.Identity.Roles;
using PearlDesk.Tenants.Application;
using PearlDesk.Tenants.Application.Queries;

namespace PearlDesk.Tenants.Endpoints;

public class TenantGetByIdEndpoint(ISender sender) : EndpointWithoutRequest<TenantResponse>
{
    public override void Configure()
    {
        Get("/tenants/{id}");
        Roles(DomainRoles.SuperAdmin);
        Version(1);
        Summary(s => s.Summary = "Get a tenant by ID");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await sender.Send(new GetTenantByIdQuery(id), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendOkAsync(result.Value, ct);
    }
}
