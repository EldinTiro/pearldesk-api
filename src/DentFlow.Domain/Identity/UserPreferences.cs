using DentFlow.Domain.Common;

namespace DentFlow.Domain.Identity;

public class UserPreferences : BaseEntity
{
    public Guid UserId { get; set; }
    public string Theme { get; set; } = "light";
    public string Language { get; set; } = "en";
    public string TimeFormat { get; set; } = "24h";
    public string DefaultCalendarView { get; set; } = "week";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
