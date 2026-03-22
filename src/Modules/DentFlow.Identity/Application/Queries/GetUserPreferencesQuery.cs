using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DentFlow.Domain.Identity;
using DentFlow.Infrastructure.Persistence;

namespace DentFlow.Identity.Application.Queries;

// ── Query: user preferences ────────────────────────────────────────────────

public record GetUserPreferencesQuery(Guid UserId) : IRequest<ErrorOr<UserPreferencesResult>>;

public record UserPreferencesResult(
    string Theme,
    string Language,
    string TimeFormat,
    string DefaultCalendarView);

public class GetUserPreferencesQueryHandler(ApplicationDbContext db)
    : IRequestHandler<GetUserPreferencesQuery, ErrorOr<UserPreferencesResult>>
{
    public async Task<ErrorOr<UserPreferencesResult>> Handle(GetUserPreferencesQuery query, CancellationToken ct)
    {
        var prefs = await db.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == query.UserId, ct);

        if (prefs is null)
        {
            // Return defaults without persisting — they'll be created on first save
            return new UserPreferencesResult("light", "en", "24h", "week");
        }

        return new UserPreferencesResult(
            prefs.Theme,
            prefs.Language,
            prefs.TimeFormat,
            prefs.DefaultCalendarView);
    }
}
