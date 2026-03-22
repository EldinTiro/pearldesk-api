using DentFlow.Application.Common.Interfaces;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace DentFlow.Reporting.Endpoints;

public class DashboardStatsEndpoint(IDashboardService dashboardService)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/dashboard/stats");
        Roles("SuperAdmin", "ClinicOwner", "ClinicAdmin", "Admin", "Dentist", "Hygienist", "Receptionist", "BillingStaff", "OfficeManager");
        Version(1);
        Summary(s =>
        {
            s.Summary     = "Get dashboard KPI summary stats for the current tenant";
            s.Description = "Returns revenue, outstanding balance, new patients, appointment completion trends, and recall metrics.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var stats = await dashboardService.GetStatsAsync(ct);
        await SendOkAsync(stats, ct);
    }
}
