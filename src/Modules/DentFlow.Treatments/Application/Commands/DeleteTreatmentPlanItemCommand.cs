using ErrorOr;
using MediatR;

namespace DentFlow.Treatments.Application.Commands;

public record DeleteTreatmentPlanItemCommand(Guid ItemId) : IRequest<ErrorOr<Deleted>>;
