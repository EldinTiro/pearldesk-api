﻿using Microsoft.Extensions.DependencyInjection;

namespace DentFlow.Appointments;

/// <summary>
/// Appointment module services are registered via DentFlow.Infrastructure.DependencyInjection.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddAppointmentsModule(this IServiceCollection services) => services;
}

