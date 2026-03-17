using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Commands;

public record AddTreatmentPlanItemCommand(
    Guid TreatmentPlanId,
    int? ToothNumber,
    string? Surface,
    string? CdtCode,
    string Description,
    decimal Fee,
    TreatmentPlanItemStatus Status
) : IRequest<ErrorOr<TreatmentPlanItemResponse>>;
