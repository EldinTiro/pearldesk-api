using Microsoft.Extensions.DependencyInjection;

namespace DentFlow.Staff;

/// <summary>
/// Staff module services are registered via DentFlow.Infrastructure.DependencyInjection.
/// This class is kept as a placeholder for any future module-specific registrations.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddStaffModule(this IServiceCollection services) => services;
}

