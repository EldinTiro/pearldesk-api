using DentFlow.Treatments.Domain;

namespace DentFlow.Treatments.Application;

public record TreatmentPlanItemResponse(
    Guid Id,
    Guid TreatmentPlanId,
    int? ToothNumber,
    string? Surface,
    string? CdtCode,
    string Description,
    decimal Fee,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static TreatmentPlanItemResponse FromEntity(TreatmentPlanItem i) =>
        new(i.Id, i.TreatmentPlanId, i.ToothNumber, i.Surface, i.CdtCode,
            i.Description, i.Fee, i.Status.ToString(), i.CreatedAt, i.UpdatedAt);
}

public record TreatmentPlanResponse(
    Guid Id,
    Guid PatientId,
    string Title,
    string? Notes,
    string Status,
    IReadOnlyList<TreatmentPlanItemResponse> Items,
    decimal TotalFee,
    DateTime CreatedAt,
    DateTime? UpdatedAt)
{
    public static TreatmentPlanResponse FromEntity(TreatmentPlan p) =>
        new(p.Id, p.PatientId, p.Title, p.Notes, p.Status.ToString(),
            p.Items.Select(TreatmentPlanItemResponse.FromEntity).ToList().AsReadOnly(),
            p.Items.Sum(i => i.Fee),
            p.CreatedAt, p.UpdatedAt);
}
