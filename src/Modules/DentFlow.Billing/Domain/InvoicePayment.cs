using DentFlow.Domain.Common;

namespace DentFlow.Billing.Domain;

public class InvoicePayment : TenantAuditableEntity
{
    public Guid InvoiceId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentMethod Method { get; private set; }
    public DateTime PaidAt { get; private set; }
    public string? Reference { get; private set; }
    public string? Notes { get; private set; }

    private InvoicePayment() { }

    public static InvoicePayment Create(
        Guid invoiceId,
        decimal amount,
        PaymentMethod method,
        DateTime paidAt,
        string? reference,
        string? notes) =>
        new InvoicePayment
        {
            InvoiceId = invoiceId,
            Amount = amount,
            Method = method,
            PaidAt = paidAt,
            Reference = reference,
            Notes = notes
        };
}
