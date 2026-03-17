﻿using Microsoft.Extensions.DependencyInjection;

namespace DentFlow.Patients;

/// <summary>
/// Patient module services are registered via DentFlow.Infrastructure.DependencyInjection.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPatientsModule(this IServiceCollection services) => services;
}

