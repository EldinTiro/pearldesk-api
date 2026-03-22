using Microsoft.Extensions.DependencyInjection;

namespace DentFlow.Billing;

public static class DependencyInjection
{
    public static IServiceCollection AddBilling(this IServiceCollection services) => services;
}
