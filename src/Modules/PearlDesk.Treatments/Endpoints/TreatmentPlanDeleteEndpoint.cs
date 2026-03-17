using FastEndpoints;
using MediatR;
using PearlDesk.Treatments.Application.Commands;

namespace PearlDesk.Treatments.Endpoints;

public class TreatmentPlanDeleteEndpoint(ISender sender) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/treatment-plans/{id}");
        Roles("ClinicOwner", "ClinicAdmin", "Dentist", "Hygienist", "SuperAdmin");
        Version(1);
        Summary(s => s.Summary = "Soft-delete a treatment plan");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");

        var result = await sender.Send(new DeleteTreatmentPlanCommand(id), ct);

        if (result.IsError)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
