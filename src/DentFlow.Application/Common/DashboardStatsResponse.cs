namespace DentFlow.Application.Common;

public sealed record DashboardStatsResponse(
    decimal RevenueThisMonth,
    decimal OutstandingBalance,
    int NewPatientsThisMonth,
    int AppointmentsCompletedThisWeek,
    int AppointmentsCompletedLastWeek,
    int RecallDueSoon,
    int RecallOverdue);
