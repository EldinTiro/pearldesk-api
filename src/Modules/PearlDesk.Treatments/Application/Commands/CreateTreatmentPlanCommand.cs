using ErrorOr;
using MediatR;

namespace PearlDesk.Treatments.Application.Commands;

public record CreateTreatmentPlanCommand(
    Guid PatientId,
    string Title,
    string? Notes
) : IRequest<ErrorOr<TreatmentPlanResponse>>;
