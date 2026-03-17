using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Application.Interfaces;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Commands;

public class AddTreatmentPlanItemCommandHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<AddTreatmentPlanItemCommand, ErrorOr<TreatmentPlanItemResponse>>
{
    public async Task<ErrorOr<TreatmentPlanItemResponse>> Handle(
        AddTreatmentPlanItemCommand command, CancellationToken ct)
    {
        var plan = await repository.GetByIdAsync(command.TreatmentPlanId, ct);
        if (plan is null) return TreatmentPlanErrors.NotFound;

        var item = TreatmentPlanItem.Create(
            command.TreatmentPlanId,
            command.ToothNumber,
            command.Surface,
            command.CdtCode,
            command.Description,
            command.Fee,
            command.Status);

        await repository.AddItemAsync(item, ct);
        return TreatmentPlanItemResponse.FromEntity(item);
    }
}
