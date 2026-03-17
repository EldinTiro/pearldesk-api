using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentFlow.Domain.Identity;
using DentFlow.Identity.Application;

namespace DentFlow.Identity.Application.Queries;

public record ListTenantUsersQuery(
    Guid TenantId,
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 20) : IRequest<ErrorOr<ListUsersResult>>;

public class ListTenantUsersQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<ListTenantUsersQuery, ErrorOr<ListUsersResult>>
{
    public async Task<ErrorOr<ListUsersResult>> Handle(ListTenantUsersQuery query, CancellationToken ct)
    {
        var q = userManager.Users.Where(u => u.TenantId == query.TenantId);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.ToLower();
            q = q.Where(u =>
                u.Email!.ToLower().Contains(term) ||
                u.FirstName.ToLower().Contains(term) ||
                u.LastName.ToLower().Contains(term));
        }

        var total = await q.CountAsync(ct);
        var users = await q
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        var responses = new List<UserAdminResponse>();
        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            responses.Add(UserAdminResponse.FromUser(user, roles.FirstOrDefault()));
        }

        return new ListUsersResult(responses, total, query.Page, query.PageSize);
    }
}
