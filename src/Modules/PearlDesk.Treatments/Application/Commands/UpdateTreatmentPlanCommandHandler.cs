using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Application.Interfaces;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Commands;

public class UpdateTreatmentPlanCommandHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<UpdateTreatmentPlanCommand, ErrorOr<TreatmentPlanResponse>>
{
    public async Task<ErrorOr<TreatmentPlanResponse>> Handle(
        UpdateTreatmentPlanCommand command, CancellationToken ct)
    {
        var plan = await repository.GetByIdWithItemsAsync(command.Id, ct);
        if (plan is null) return TreatmentPlanErrors.NotFound;

        plan.Update(command.Title, command.Notes, command.Status);
        await repository.UpdateAsync(plan, ct);
        return TreatmentPlanResponse.FromEntity(plan);
    }
}
