using DentFlow.Domain.Common;

namespace DentFlow.Billing.Domain;

public class InvoiceLineItem : TenantAuditableEntity
{
    public Guid InvoiceId { get; private set; }
    public string Description { get; private set; } = null!;
    public string? CdtCode { get; private set; }
    public int? ToothNumber { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitFee { get; private set; }
    public Guid? TreatmentPlanItemId { get; private set; }

    public decimal LineTotal => Quantity * UnitFee;

    private InvoiceLineItem() { }

    public static InvoiceLineItem Create(
        Guid invoiceId,
        string description,
        string? cdtCode,
        int? toothNumber,
        int quantity,
        decimal unitFee,
        Guid? treatmentPlanItemId = null) =>
        new InvoiceLineItem
        {
            InvoiceId = invoiceId,
            Description = description,
            CdtCode = cdtCode,
            ToothNumber = toothNumber,
            Quantity = quantity,
            UnitFee = unitFee,
            TreatmentPlanItemId = treatmentPlanItemId
        };

    public void Update(string description, string? cdtCode, int? toothNumber, int quantity, decimal unitFee)
    {
        Description = description;
        CdtCode = cdtCode;
        ToothNumber = toothNumber;
        Quantity = quantity;
        UnitFee = unitFee;
        SetUpdated();
    }
}
