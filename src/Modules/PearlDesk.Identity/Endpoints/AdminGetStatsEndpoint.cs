using FastEndpoints;
using MediatR;
using DomainRoles = PearlDesk.Domain.Identity.Roles;
using PearlDesk.Identity.Application.Queries;

namespace PearlDesk.Identity.Endpoints;

public class AdminGetStatsEndpoint(ISender sender) : EndpointWithoutRequest<PlatformStatsResponse>
{
    public override void Configure()
    {
        Get("/admin/stats");
        Roles(DomainRoles.SuperAdmin);
        Version(1);
        Summary(s => s.Summary = "Get platform-wide statistics");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await sender.Send(new GetPlatformStatsQuery(), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendOkAsync(result.Value, ct);
    }
}
