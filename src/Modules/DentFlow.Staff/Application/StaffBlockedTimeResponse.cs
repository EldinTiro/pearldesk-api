using DentFlow.Staff.Domain;

namespace DentFlow.Staff.Application;

public record StaffBlockedTimeResponse(
    Guid Id,
    Guid StaffMemberId,
    DateTime StartAt,
    DateTime EndAt,
    string? AbsenceType,
    string? Notes)
{
    public static StaffBlockedTimeResponse FromEntity(StaffBlockedTime b) => new(
        b.Id,
        b.StaffMemberId,
        b.StartAt,
        b.EndAt,
        b.AbsenceType,
        b.Notes);
}
