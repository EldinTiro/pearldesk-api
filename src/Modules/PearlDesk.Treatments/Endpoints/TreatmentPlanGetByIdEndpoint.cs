using FastEndpoints;
using MediatR;
using PearlDesk.Treatments.Application;
using PearlDesk.Treatments.Application.Queries;

namespace PearlDesk.Treatments.Endpoints;

public class TreatmentPlanGetByIdEndpoint(ISender sender)
    : EndpointWithoutRequest<TreatmentPlanResponse>
{
    public override void Configure()
    {
        Get("/treatment-plans/{id}");
        Roles("ClinicOwner", "ClinicAdmin", "Dentist", "Hygienist", "Receptionist", "SuperAdmin");
        Version(1);
        Summary(s => s.Summary = "Get a treatment plan by ID (includes all items)");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var result = await sender.Send(new GetTreatmentPlanByIdQuery(id), ct);

        if (result.IsError)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendOkAsync(result.Value, ct);
    }
}
