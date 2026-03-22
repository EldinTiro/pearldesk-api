using DentFlow.Domain.Common;

namespace DentFlow.Billing.Domain;

public class Invoice : TenantAuditableEntity
{
    public Guid PatientId { get; private set; }
    public string InvoiceNumber { get; private set; } = null!;
    public InvoiceStatus Status { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public DateTime? DueDate { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<InvoiceLineItem> _lineItems = [];
    public IReadOnlyList<InvoiceLineItem> LineItems => _lineItems.AsReadOnly();

    private readonly List<InvoicePayment> _payments = [];
    public IReadOnlyList<InvoicePayment> Payments => _payments.AsReadOnly();

    public decimal SubTotal => _lineItems.Sum(i => i.LineTotal);
    public decimal PaidAmount => _payments.Sum(p => p.Amount);
    public decimal BalanceDue => SubTotal - PaidAmount;

    private Invoice() { }

    public static Invoice Create(Guid patientId, string invoiceNumber, DateTime? dueDate, string? notes) =>
        new Invoice
        {
            PatientId = patientId,
            InvoiceNumber = invoiceNumber,
            Status = InvoiceStatus.Draft,
            IssuedAt = DateTime.UtcNow,
            DueDate = dueDate,
            Notes = notes
        };

    public void Update(DateTime? dueDate, string? notes)
    {
        DueDate = dueDate;
        Notes = notes;
        SetUpdated();
    }

    public void AddLineItem(InvoiceLineItem item) => _lineItems.Add(item);
    public void RemoveLineItem(InvoiceLineItem item) => _lineItems.Remove(item);

    public void MarkAsSent()
    {
        if (Status == InvoiceStatus.Draft)
            Status = InvoiceStatus.Sent;
        SetUpdated();
    }

    public void AddPayment(InvoicePayment payment)
    {
        _payments.Add(payment);
        RecalculateStatus();
        SetUpdated();
    }

    public void Void()
    {
        Status = InvoiceStatus.Void;
        SetUpdated();
    }

    private void RecalculateStatus()
    {
        if (BalanceDue <= 0)
            Status = InvoiceStatus.Paid;
        else if (PaidAmount > 0)
            Status = InvoiceStatus.PartiallyPaid;
    }
}
