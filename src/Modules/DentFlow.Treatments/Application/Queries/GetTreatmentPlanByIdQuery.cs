using ErrorOr;
using MediatR;

namespace DentFlow.Treatments.Application.Queries;

public record GetTreatmentPlanByIdQuery(Guid Id) : IRequest<ErrorOr<TreatmentPlanResponse>>;
