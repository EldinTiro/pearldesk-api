using PearlDesk.Domain.Tenants;

namespace PearlDesk.Tenants.Application;

public record TenantResponse(
    Guid Id,
    string Slug,
    string Name,
    string Plan,
    DateTime? PlanExpiresAt,
    bool IsActive,
    DateTime? OnboardingCompletedAt,
    DateTime CreatedAt,
    string? LogoBase64)
{
    public static TenantResponse FromEntity(Tenant t) =>
        new(t.Id, t.Slug, t.Name, t.Plan, t.PlanExpiresAt, t.IsActive, t.OnboardingCompletedAt, t.CreatedAt, t.LogoBase64);
}

/// <summary>
/// Returned only on tenant creation — exposes the generated temp password once.
/// </summary>
public record TenantCreatedResponse(
    Guid Id,
    string Slug,
    string Name,
    string Plan,
    bool IsActive,
    DateTime CreatedAt,
    string OwnerEmail,
    string TempPassword,
    string? LogoBase64)
{
    public static TenantCreatedResponse FromEntity(Tenant t, string ownerEmail, string tempPassword) =>
        new(t.Id, t.Slug, t.Name, t.Plan, t.IsActive, t.CreatedAt, ownerEmail, tempPassword, t.LogoBase64);
}
