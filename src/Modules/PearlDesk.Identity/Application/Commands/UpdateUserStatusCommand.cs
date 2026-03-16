using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PearlDesk.Domain.Identity;

namespace PearlDesk.Identity.Application.Commands;

public record UpdateUserStatusCommand(Guid UserId, bool IsActive) : IRequest<ErrorOr<Updated>>;

public class UpdateUserStatusCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<UpdateUserStatusCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserStatusCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(command.UserId.ToString());
        if (user is null) return IdentityErrors.UserNotFound;

        user.IsActive = command.IsActive;
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

        return Result.Updated;
    }
}
