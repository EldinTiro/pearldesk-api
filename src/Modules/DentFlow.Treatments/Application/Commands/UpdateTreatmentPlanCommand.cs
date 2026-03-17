using ErrorOr;
using MediatR;
using DentFlow.Treatments.Domain;

namespace DentFlow.Treatments.Application.Commands;

public record UpdateTreatmentPlanCommand(
    Guid Id,
    string Title,
    string? Notes,
    TreatmentPlanStatus Status
) : IRequest<ErrorOr<TreatmentPlanResponse>>;
