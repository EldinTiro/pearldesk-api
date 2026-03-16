using FastEndpoints;
using MediatR;
using DomainRoles = PearlDesk.Domain.Identity.Roles;
using PearlDesk.Identity.Application.Commands;

namespace PearlDesk.Identity.Endpoints;

public class AdminChangeUserRoleEndpoint(ISender sender) : Endpoint<ChangeUserRoleRequest>
{
    public override void Configure()
    {
        Put("/admin/users/{id}/role");
        Roles(DomainRoles.SuperAdmin);
        Version(1);
        Summary(s => s.Summary = "Change a user's role");
    }

    public override async Task HandleAsync(ChangeUserRoleRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await sender.Send(new ChangeUserRoleCommand(id, req.Role), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendNoContentAsync(ct);
    }
}

public record ChangeUserRoleRequest(string Role);
