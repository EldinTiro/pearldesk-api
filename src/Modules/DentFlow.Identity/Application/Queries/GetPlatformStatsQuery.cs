using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DentFlow.Domain.Identity;
using DentFlow.Infrastructure.Persistence;

namespace DentFlow.Identity.Application.Queries;

public record GetPlatformStatsQuery : IRequest<ErrorOr<PlatformStatsResponse>>;

public record PlatformStatsResponse(
    int TotalTenants,
    int ActiveTenants,
    int InactiveTenants,
    int TotalUsers,
    IReadOnlyList<RoleCount> UsersByRole,
    IReadOnlyList<PlanCount> PlanDistribution);

public record RoleCount(string Role, int Count);
public record PlanCount(string Plan, int Count);

public class GetPlatformStatsQueryHandler(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetPlatformStatsQuery, ErrorOr<PlatformStatsResponse>>
{
    public async Task<ErrorOr<PlatformStatsResponse>> Handle(
        GetPlatformStatsQuery query, CancellationToken ct)
    {
        var totalTenants   = await dbContext.Tenants.CountAsync(ct);
        var activeTenants  = await dbContext.Tenants.CountAsync(t => t.IsActive, ct);

        var totalUsers = await userManager.Users.CountAsync(ct);

        // Role distribution — fetch lightweight projections then group in memory
        var allUserRoles = await dbContext.UserRoles
            .Join(dbContext.Roles,
                ur => ur.RoleId,
                r  => r.Id,
                (ur, r) => r.Name)
            .ToListAsync(ct);

        var usersByRole = allUserRoles
            .Where(name => name is not null)
            .GroupBy(name => name!)
            .Select(g => new RoleCount(g.Key, g.Count()))
            .ToList();

        // Plan distribution
        var planDistribution = await dbContext.Tenants
            .GroupBy(t => t.Plan)
            .Select(g => new PlanCount(g.Key, g.Count()))
            .ToListAsync(ct);

        return new PlatformStatsResponse(
            TotalTenants:   totalTenants,
            ActiveTenants:  activeTenants,
            InactiveTenants: totalTenants - activeTenants,
            TotalUsers:     totalUsers,
            UsersByRole:    usersByRole,
            PlanDistribution: planDistribution);
    }
}
