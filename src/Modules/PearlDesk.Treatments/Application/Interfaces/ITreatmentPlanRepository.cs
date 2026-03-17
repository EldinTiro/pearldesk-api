using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Interfaces;

public interface ITreatmentPlanRepository
{
    Task<TreatmentPlan?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<TreatmentPlan?> GetByIdWithItemsAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<TreatmentPlan> Items, int Total)> ListByPatientAsync(
        Guid patientId,
        TreatmentPlanStatus? status,
        int page,
        int pageSize,
        CancellationToken ct = default);
    Task AddAsync(TreatmentPlan plan, CancellationToken ct = default);
    Task UpdateAsync(TreatmentPlan plan, CancellationToken ct = default);
    Task SoftDeleteAsync(TreatmentPlan plan, CancellationToken ct = default);

    Task<TreatmentPlanItem?> GetItemByIdAsync(Guid itemId, CancellationToken ct = default);
    Task AddItemAsync(TreatmentPlanItem item, CancellationToken ct = default);
    Task UpdateItemAsync(TreatmentPlanItem item, CancellationToken ct = default);
    Task SoftDeleteItemAsync(TreatmentPlanItem item, CancellationToken ct = default);
}
