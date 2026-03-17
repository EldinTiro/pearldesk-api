using ErrorOr;
using MediatR;

namespace PearlDesk.Treatments.Application.Queries;

public record GetTreatmentPlanByIdQuery(Guid Id) : IRequest<ErrorOr<TreatmentPlanResponse>>;
