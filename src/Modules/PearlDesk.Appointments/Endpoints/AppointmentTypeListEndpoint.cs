using FastEndpoints;
using MediatR;
using PearlDesk.Appointments.Application;
using PearlDesk.Appointments.Application.Queries;

namespace PearlDesk.Appointments.Endpoints;

public class AppointmentTypeListEndpoint(ISender sender)
    : EndpointWithoutRequest<IReadOnlyList<AppointmentTypeResponse>>
{
    public override void Configure()
    {
        Get("/appointment-types");
        Roles("ClinicOwner", "ClinicAdmin", "Receptionist", "Dentist", "Hygienist", "SuperAdmin");
        Version(1);
        Summary(s => s.Summary = "List all appointment types for the current tenant");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await sender.Send(new ListAppointmentTypesQuery(), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendOkAsync(result.Value, ct);
    }
}
