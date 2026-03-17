using ErrorOr;
using MediatR;
using DentFlow.Treatments.Application.Interfaces;
using DentFlow.Treatments.Domain;

namespace DentFlow.Treatments.Application.Queries;

public class GetTreatmentPlanByIdQueryHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<GetTreatmentPlanByIdQuery, ErrorOr<TreatmentPlanResponse>>
{
    public async Task<ErrorOr<TreatmentPlanResponse>> Handle(
        GetTreatmentPlanByIdQuery query, CancellationToken ct)
    {
        var plan = await repository.GetByIdWithItemsAsync(query.Id, ct);
        if (plan is null) return TreatmentPlanErrors.NotFound;
        return TreatmentPlanResponse.FromEntity(plan);
    }
}
