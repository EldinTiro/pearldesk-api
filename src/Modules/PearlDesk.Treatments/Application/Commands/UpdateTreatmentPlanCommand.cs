using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Commands;

public record UpdateTreatmentPlanCommand(
    Guid Id,
    string Title,
    string? Notes,
    TreatmentPlanStatus Status
) : IRequest<ErrorOr<TreatmentPlanResponse>>;
