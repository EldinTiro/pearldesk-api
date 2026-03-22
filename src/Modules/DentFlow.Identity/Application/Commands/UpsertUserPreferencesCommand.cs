using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DentFlow.Domain.Identity;
using DentFlow.Infrastructure.Persistence;

namespace DentFlow.Identity.Application.Commands;

// ── Command: upsert user preferences ──────────────────────────────────────

public record UpsertUserPreferencesCommand(
    Guid UserId,
    string Theme,
    string Language,
    string TimeFormat,
    string DefaultCalendarView) : IRequest<ErrorOr<Updated>>;

public class UpsertUserPreferencesCommandHandler(ApplicationDbContext db)
    : IRequestHandler<UpsertUserPreferencesCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpsertUserPreferencesCommand command, CancellationToken ct)
    {
        var prefs = await db.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == command.UserId, ct);

        if (prefs is null)
        {
            prefs = new UserPreferences { UserId = command.UserId };
            db.UserPreferences.Add(prefs);
        }

        prefs.Theme = command.Theme;
        prefs.Language = command.Language;
        prefs.TimeFormat = command.TimeFormat;
        prefs.DefaultCalendarView = command.DefaultCalendarView;
        prefs.UpdatedAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return Result.Updated;
    }
}
