using DentFlow.Domain.Common;

namespace DentFlow.Staff.Domain;

public class StaffBlockedTime : TenantAuditableEntity
{
    public Guid StaffMemberId { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string? AbsenceType { get; private set; }
    public string? Notes { get; private set; }

    // Navigation
    public StaffMember StaffMember { get; private set; } = default!;

    private StaffBlockedTime() { }

    public static StaffBlockedTime Create(
        Guid staffMemberId,
        DateTime startAt,
        DateTime endAt,
        string? absenceType = null,
        string? notes = null)
    {
        return new StaffBlockedTime
        {
            StaffMemberId = staffMemberId,
            StartAt = startAt,
            EndAt = endAt,
            AbsenceType = absenceType,
            Notes = notes
        };
    }
}

