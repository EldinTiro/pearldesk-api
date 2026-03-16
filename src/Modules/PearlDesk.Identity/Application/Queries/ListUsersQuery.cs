using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PearlDesk.Domain.Identity;

namespace PearlDesk.Identity.Application.Queries;

public record ListUsersQuery(
    Guid? TenantId,
    string? Role,
    string? SearchTerm,
    int Page = 1,
    int PageSize = 20) : IRequest<ErrorOr<ListUsersResult>>;

public record ListUsersResult(IReadOnlyList<UserAdminResponse> Items, int TotalCount, int Page, int PageSize);

public class ListUsersQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<ListUsersQuery, ErrorOr<ListUsersResult>>
{
    public async Task<ErrorOr<ListUsersResult>> Handle(ListUsersQuery query, CancellationToken cancellationToken)
    {
        // Cross-tenant query — bypass global query filter
        var usersQuery = userManager.Users.AsQueryable();

        if (query.TenantId.HasValue)
            usersQuery = usersQuery.Where(u => u.TenantId == query.TenantId);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.ToLower();
            usersQuery = usersQuery.Where(u =>
                u.Email!.ToLower().Contains(term) ||
                u.FirstName.ToLower().Contains(term) ||
                u.LastName.ToLower().Contains(term));
        }

        var total = await usersQuery.CountAsync(cancellationToken);
        var users = await usersQuery
            .OrderByDescending(u => u.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var responses = new List<UserAdminResponse>();
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(query.Role) &&
                !roles.Contains(query.Role, StringComparer.OrdinalIgnoreCase))
                continue;

            responses.Add(UserAdminResponse.FromUser(user, primaryRole));
        }

        return new ListUsersResult(responses, total, query.Page, query.PageSize);
    }
}
