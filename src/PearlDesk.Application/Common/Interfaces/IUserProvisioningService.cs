using ErrorOr;

namespace PearlDesk.Application.Common.Interfaces;

/// <summary>
/// Abstracts user creation from the Identity module so that other modules
/// (e.g. Tenants) can provision users without a direct module-to-module reference.
/// Implementation lives in PearlDesk.Infrastructure.
/// </summary>
public interface IUserProvisioningService
{
    Task<ErrorOr<Guid>> CreateUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        string role,
        Guid tenantId,
        CancellationToken ct = default);
}
