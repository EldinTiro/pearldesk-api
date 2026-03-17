using DentFlow.Domain.Common;

namespace DentFlow.Treatments.Domain;

public class TreatmentPlanItem : TenantAuditableEntity
{
    public Guid TreatmentPlanId { get; private set; }
    public int? ToothNumber { get; private set; }
    public string? Surface { get; private set; }
    public string? CdtCode { get; private set; }
    public string Description { get; private set; } = null!;
    public decimal Fee { get; private set; }
    public TreatmentPlanItemStatus Status { get; private set; }

    private TreatmentPlanItem() { }

    public static TreatmentPlanItem Create(
        Guid treatmentPlanId,
        int? toothNumber,
        string? surface,
        string? cdtCode,
        string description,
        decimal fee,
        TreatmentPlanItemStatus status)
    {
        return new TreatmentPlanItem
        {
            TreatmentPlanId = treatmentPlanId,
            ToothNumber = toothNumber,
            Surface = surface,
            CdtCode = cdtCode,
            Description = description,
            Fee = fee,
            Status = status,
        };
    }

    public void Update(
        int? toothNumber,
        string? surface,
        string? cdtCode,
        string description,
        decimal fee,
        TreatmentPlanItemStatus status)
    {
        ToothNumber = toothNumber;
        Surface = surface;
        CdtCode = cdtCode;
        Description = description;
        Fee = fee;
        Status = status;
        SetUpdated();
    }
}
