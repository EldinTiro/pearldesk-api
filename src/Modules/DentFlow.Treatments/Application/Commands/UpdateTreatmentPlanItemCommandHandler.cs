using ErrorOr;
using MediatR;
using DentFlow.Treatments.Application.Interfaces;
using DentFlow.Treatments.Domain;

namespace DentFlow.Treatments.Application.Commands;

public class UpdateTreatmentPlanItemCommandHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<UpdateTreatmentPlanItemCommand, ErrorOr<TreatmentPlanItemResponse>>
{
    public async Task<ErrorOr<TreatmentPlanItemResponse>> Handle(
        UpdateTreatmentPlanItemCommand command, CancellationToken ct)
    {
        var item = await repository.GetItemByIdAsync(command.ItemId, ct);
        if (item is null) return TreatmentPlanErrors.ItemNotFound;

        item.Update(command.ToothNumber, command.Surface, command.CdtCode,
            command.Description, command.Fee, command.Status);

        await repository.UpdateItemAsync(item, ct);
        return TreatmentPlanItemResponse.FromEntity(item);
    }
}
