using ErrorOr;
using MediatR;
using DentFlow.Treatments.Application.Interfaces;
using DentFlow.Treatments.Domain;

namespace DentFlow.Treatments.Application.Commands;

public class DeleteTreatmentPlanCommandHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<DeleteTreatmentPlanCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(
        DeleteTreatmentPlanCommand command, CancellationToken ct)
    {
        var plan = await repository.GetByIdAsync(command.Id, ct);
        if (plan is null) return TreatmentPlanErrors.NotFound;

        await repository.SoftDeleteAsync(plan, ct);
        return Result.Deleted;
    }
}
