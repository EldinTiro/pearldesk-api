using Microsoft.EntityFrameworkCore;
using PearlDesk.Domain.Tenants;
using PearlDesk.Infrastructure.Persistence;
using PearlDesk.Tenants.Application.Interfaces;

namespace PearlDesk.Infrastructure.Persistence.Repositories;

public class TenantRepository(ApplicationDbContext dbContext) : ITenantRepository
{
    public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await dbContext.Tenants.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<Tenant?> GetBySlugAsync(string slug, CancellationToken ct = default) =>
        await dbContext.Tenants.FirstOrDefaultAsync(
            t => t.Slug == slug.ToLowerInvariant().Trim(), ct);

    public async Task<(IReadOnlyList<Tenant> Items, int Total)> ListAsync(
        string? searchTerm, bool? isActive, int page, int pageSize, CancellationToken ct = default)
    {
        var query = dbContext.Tenants.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(t =>
                t.Name.ToLower().Contains(term) ||
                t.Slug.ToLower().Contains(term));
        }

        if (isActive.HasValue)
            query = query.Where(t => t.IsActive == isActive.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task AddAsync(Tenant tenant, CancellationToken ct = default)
    {
        await dbContext.Tenants.AddAsync(tenant, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken ct = default)
    {
        dbContext.Tenants.Update(tenant);
        await dbContext.SaveChangesAsync(ct);
    }
}
