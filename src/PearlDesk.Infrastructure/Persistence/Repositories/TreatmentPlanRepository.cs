using Microsoft.EntityFrameworkCore;
using PearlDesk.Infrastructure.Persistence;
using PearlDesk.Treatments.Application.Interfaces;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Infrastructure.Persistence.Repositories;

public class TreatmentPlanRepository(ApplicationDbContext dbContext) : ITreatmentPlanRepository
{
    public Task<TreatmentPlan?> GetByIdAsync(Guid id, CancellationToken ct) =>
        dbContext.Set<TreatmentPlan>().FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<TreatmentPlan?> GetByIdWithItemsAsync(Guid id, CancellationToken ct) =>
        dbContext.Set<TreatmentPlan>()
            .Include(p => p.Items)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<(IReadOnlyList<TreatmentPlan> Items, int Total)> ListByPatientAsync(
        Guid patientId, TreatmentPlanStatus? status, int page, int pageSize, CancellationToken ct)
    {
        var query = dbContext.Set<TreatmentPlan>()
            .Include(p => p.Items)
            .Where(p => p.PatientId == patientId);

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    public async Task AddAsync(TreatmentPlan plan, CancellationToken ct)
    {
        await dbContext.Set<TreatmentPlan>().AddAsync(plan, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TreatmentPlan plan, CancellationToken ct)
    {
        dbContext.Set<TreatmentPlan>().Update(plan);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(TreatmentPlan plan, CancellationToken ct)
    {
        plan.SoftDelete();
        await dbContext.SaveChangesAsync(ct);
    }

    public Task<TreatmentPlanItem?> GetItemByIdAsync(Guid itemId, CancellationToken ct) =>
        dbContext.Set<TreatmentPlanItem>().FirstOrDefaultAsync(i => i.Id == itemId, ct);

    public async Task AddItemAsync(TreatmentPlanItem item, CancellationToken ct)
    {
        await dbContext.Set<TreatmentPlanItem>().AddAsync(item, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateItemAsync(TreatmentPlanItem item, CancellationToken ct)
    {
        dbContext.Set<TreatmentPlanItem>().Update(item);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteItemAsync(TreatmentPlanItem item, CancellationToken ct)
    {
        item.SoftDelete();
        await dbContext.SaveChangesAsync(ct);
    }
}
