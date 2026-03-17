using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Application.Interfaces;

namespace PearlDesk.Treatments.Application.Queries;

public class ListTreatmentPlansQueryHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<ListTreatmentPlansQuery, ErrorOr<PagedResult<TreatmentPlanResponse>>>
{
    public async Task<ErrorOr<PagedResult<TreatmentPlanResponse>>> Handle(
        ListTreatmentPlansQuery query, CancellationToken ct)
    {
        var (items, total) = await repository.ListByPatientAsync(
            query.PatientId, query.Status, query.Page, query.PageSize, ct);

        var responses = items.Select(TreatmentPlanResponse.FromEntity).ToList();
        return new PagedResult<TreatmentPlanResponse>(responses, total, query.Page, query.PageSize);
    }
}
