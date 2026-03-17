using ErrorOr;
using MediatR;

namespace PearlDesk.Treatments.Application.Commands;

public record DeleteTreatmentPlanCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;
