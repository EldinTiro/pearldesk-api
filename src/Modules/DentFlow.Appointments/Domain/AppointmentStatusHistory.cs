using DentFlow.Domain.Common;

namespace DentFlow.Appointments.Domain;

public class AppointmentStatusHistory : TenantAuditableEntity
{
    public Guid AppointmentId { get; private set; }
    public string? FromStatus { get; private set; }
    public string ToStatus { get; private set; } = default!;
    public Guid? ChangedByUserId { get; private set; }
    public string? Reason { get; private set; }
    public bool IsOverride { get; private set; }
    public DateTime ChangedAt { get; private set; }

    private AppointmentStatusHistory() { }

    public static AppointmentStatusHistory Create(
        Guid appointmentId,
        string? fromStatus,
        string toStatus,
        Guid? changedByUserId = null,
        string? reason = null,
        bool isOverride = false)
    {
        return new AppointmentStatusHistory
        {
            AppointmentId = appointmentId,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            ChangedByUserId = changedByUserId,
            Reason = reason,
            IsOverride = isOverride,
            ChangedAt = DateTime.UtcNow,
        };
    }
}
