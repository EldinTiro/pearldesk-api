namespace DentFlow.Treatments.Domain;

public enum TreatmentPlanStatus
{
    Draft,
    Active,
    Completed,
    Declined
}

public enum TreatmentPlanItemStatus
{
    Planned,
    Accepted,
    Completed,
    Declined
}
