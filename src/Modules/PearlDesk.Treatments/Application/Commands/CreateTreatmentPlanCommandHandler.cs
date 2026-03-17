using ErrorOr;
using MediatR;
using PearlDesk.Treatments.Application.Interfaces;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Treatments.Application.Commands;

public class CreateTreatmentPlanCommandHandler(ITreatmentPlanRepository repository)
    : IRequestHandler<CreateTreatmentPlanCommand, ErrorOr<TreatmentPlanResponse>>
{
    public async Task<ErrorOr<TreatmentPlanResponse>> Handle(
        CreateTreatmentPlanCommand command, CancellationToken ct)
    {
        var plan = TreatmentPlan.Create(command.PatientId, command.Title, command.Notes);
        await repository.AddAsync(plan, ct);
        // Reload with items (empty list at creation, but consistent response)
        return TreatmentPlanResponse.FromEntity(plan);
    }
}
