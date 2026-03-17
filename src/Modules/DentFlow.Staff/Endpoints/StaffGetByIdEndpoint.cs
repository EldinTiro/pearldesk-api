using FastEndpoints;
using MediatR;
using DentFlow.Staff.Application;
using DentFlow.Staff.Application.Queries;

namespace DentFlow.Staff.Endpoints;

public class StaffGetByIdEndpoint(ISender sender) : EndpointWithoutRequest<StaffMemberResponse>
{
    public override void Configure()
    {
        Get("/staff/{id}");
        Roles("ClinicOwner", "ClinicAdmin", "Dentist", "Hygienist", "Receptionist", "SuperAdmin");
        Version(1);
        Summary(s => s.Summary = "Get a staff member by ID");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await sender.Send(new GetStaffMemberByIdQuery(id), ct);
        if (result.IsError) { await SendErrorsAsync(cancellation: ct); return; }
        await SendOkAsync(result.Value, ct);
    }
}
