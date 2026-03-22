namespace DentFlow.Billing.Domain;

public enum InvoiceStatus
{
    Draft,
    Sent,
    PartiallyPaid,
    Paid,
    Void
}

public enum PaymentMethod
{
    Cash,
    Card,
    Insurance,
    BankTransfer,
    Other
}
