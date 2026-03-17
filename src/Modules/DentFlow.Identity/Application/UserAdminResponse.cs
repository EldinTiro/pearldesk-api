using DentFlow.Domain.Identity;

namespace DentFlow.Identity.Application;

public record UserAdminResponse(
    Guid Id,
    string Email,
    string FullName,
    string FirstName,
    string LastName,
    string? Role,
    Guid? TenantId,
    bool IsActive,
    DateTime? LastLoginAt,
    DateTime CreatedAt)
{
    public static UserAdminResponse FromUser(ApplicationUser u, string? role) =>
        new(u.Id, u.Email!, u.FullName, u.FirstName, u.LastName,
            role, u.TenantId, u.IsActive, u.LastLoginAt, u.CreatedAt);
}
