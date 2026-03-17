using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Application.Interfaces;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Queries;

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
