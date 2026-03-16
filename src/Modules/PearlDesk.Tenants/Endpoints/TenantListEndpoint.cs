using FastEndpoints;
using MediatR;
using DomainRoles = PearlDesk.Domain.Identity.Roles;
using PearlDesk.Tenants.Application;
using PearlDesk.Tenants.Application.Queries;

namespace PearlDesk.Tenants.Endpoints;

public class TenantListEndpoint(ISender sender) : EndpointWithoutRequest<PagedResult<TenantResponse>>
{
    public override void Configure()
    {
        Get("/tenants");
        Roles(DomainRoles.SuperAdmin);
        Version(1);
        Summary(s => s.Summary = "List all tenants (SuperAdmin only)");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var searchTerm = Query<string?>("searchTerm", isRequired: false);
        var isActive = Query<bool?>("isActive", isRequired: false);
        var page = Query<int>("page", isRequired: false);
        var pageSize = Query<int>("pageSize", isRequired: false);

        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var result = await sender.Send(new ListTenantsQuery(searchTerm, isActive, page, pageSize), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendOkAsync(result.Value, ct);
    }
}
