using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DentFlow.Domain.Identity;

namespace DentFlow.Identity.Application.Commands;

public record ChangeUserRoleCommand(Guid UserId, string NewRole) : IRequest<ErrorOr<Updated>>;

public class ChangeUserRoleCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<ChangeUserRoleCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(ChangeUserRoleCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());
        if (user is null) return IdentityErrors.UserNotFound;

        if (!Roles.All.Contains(command.NewRole))
            return Error.Validation("Role.Invalid", $"'{command.NewRole}' is not a valid role.");

        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Any())
            await userManager.RemoveFromRolesAsync(user, currentRoles);

        var result = await userManager.AddToRoleAsync(user, command.NewRole);
        if (!result.Succeeded)
            return result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

        return Result.Updated;
    }
}
