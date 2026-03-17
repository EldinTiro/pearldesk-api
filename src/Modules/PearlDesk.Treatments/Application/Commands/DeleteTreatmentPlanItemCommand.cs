using ErrorOr;
using MediatR;

namespace PearlDesk.Treatments.Application.Commands;

public record DeleteTreatmentPlanItemCommand(Guid ItemId) : IRequest<ErrorOr<Deleted>>;
