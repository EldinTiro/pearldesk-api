using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Application.Interfaces;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Commands;

public class DeleteTreatmentPlanItemCommandHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<DeleteTreatmentPlanItemCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteTreatmentPlanItemCommand command, CancellationToken ct)
    {
        var item = await repository.GetItemByIdAsync(command.ItemId, ct);
        if (item is null) return TreatmentPlanErrors.ItemNotFound;

        await repository.SoftDeleteItemAsync(item, ct);
        return Result.Deleted;
    }
}
