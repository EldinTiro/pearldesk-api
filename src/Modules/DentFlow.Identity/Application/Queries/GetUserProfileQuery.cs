using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DentFlow.Domain.Identity;

namespace DentFlow.Identity.Application.Queries;

// ── Query: current user profile ────────────────────────────────────────────

public record GetUserProfileQuery(Guid UserId) : IRequest<ErrorOr<UserProfileResult>>;

public record UserProfileResult(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    string? AvatarUrl,
    DateTime? LastLoginAt);

public class GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetUserProfileQuery, ErrorOr<UserProfileResult>>
{
    public async Task<ErrorOr<UserProfileResult>> Handle(GetUserProfileQuery query, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(query.UserId.ToString());
        if (user is null)
            return IdentityErrors.UserNotFound;

        return new UserProfileResult(
            Id: user.Id,
            Email: user.Email!,
            FirstName: user.FirstName,
            LastName: user.LastName,
            FullName: user.FullName,
            AvatarUrl: user.AvatarUrl,
            LastLoginAt: user.LastLoginAt);
    }
}
