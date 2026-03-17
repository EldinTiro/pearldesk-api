using ErrorOr;
using MediatR;

namespace DentFlow.Treatments.Application.Commands;

public record CreateTreatmentPlanCommand(
    Guid PatientId,
    string Title,
    string? Notes
) : IRequest<ErrorOr<TreatmentPlanResponse>>;
