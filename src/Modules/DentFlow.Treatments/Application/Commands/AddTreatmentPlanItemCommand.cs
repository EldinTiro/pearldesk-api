using ErrorOr;
using MediatR;
using DentFlow.Treatments.Domain;

namespace DentFlow.Treatments.Application.Commands;

public record AddTreatmentPlanItemCommand(
    Guid TreatmentPlanId,
    int? ToothNumber,
    string? Surface,
    string? CdtCode,
    string Description,
    decimal Fee,
    TreatmentPlanItemStatus Status
) : IRequest<ErrorOr<TreatmentPlanItemResponse>>;
