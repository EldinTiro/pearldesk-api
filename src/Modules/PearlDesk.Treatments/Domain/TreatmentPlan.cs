using PearlDesk.Domain.Common;

namespace PearlDesk.Treatments.Domain;

public class TreatmentPlan : TenantAuditableEntity
{
    public Guid PatientId { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Notes { get; private set; }
    public TreatmentPlanStatus Status { get; private set; }

    private readonly List<TreatmentPlanItem> _items = [];
    public IReadOnlyList<TreatmentPlanItem> Items => _items.AsReadOnly();

    private TreatmentPlan() { }

    public static TreatmentPlan Create(Guid patientId, string title, string? notes)
    {
        return new TreatmentPlan
        {
            PatientId = patientId,
            Title = title,
            Notes = notes,
            Status = TreatmentPlanStatus.Draft,
        };
    }

    public void Update(string title, string? notes, TreatmentPlanStatus status)
    {
        Title = title;
        Notes = notes;
        Status = status;
        SetUpdated();
    }
}
