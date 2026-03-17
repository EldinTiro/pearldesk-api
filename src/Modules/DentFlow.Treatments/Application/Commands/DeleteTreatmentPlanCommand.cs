using ErrorOr;
using MediatR;

namespace DentFlow.Treatments.Application.Commands;

public record DeleteTreatmentPlanCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;
