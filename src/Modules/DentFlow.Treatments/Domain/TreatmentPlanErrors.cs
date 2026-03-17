using ErrorOr;

namespace DentFlow.Treatments.Domain;

public static class TreatmentPlanErrors
{
    public static readonly Error NotFound =
        Error.NotFound("TreatmentPlan.NotFound", "Treatment plan was not found.");

    public static readonly Error ItemNotFound =
        Error.NotFound("TreatmentPlan.ItemNotFound", "Treatment plan item was not found.");
}
