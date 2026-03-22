﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DentFlow.Appointments.Domain;

namespace DentFlow.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Status).HasMaxLength(30).IsRequired();
        builder.Property(a => a.StartAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(a => a.EndAt).HasColumnType("timestamptz").IsRequired();
        builder.Property(a => a.CancelledAt).HasColumnType("timestamptz");
        builder.Property(a => a.ConfirmedAt).HasColumnType("timestamptz");
        builder.Property(a => a.CheckedInAt).HasColumnType("timestamptz");
        builder.Property(a => a.StartedAt).HasColumnType("timestamptz");
        builder.Property(a => a.CompletedAt).HasColumnType("timestamptz");
        builder.Property(a => a.NoShowAt).HasColumnType("timestamptz");
        builder.Property(a => a.ReminderSentAt).HasColumnType("timestamptz");
        builder.Property(a => a.CancellationReason).HasMaxLength(255);
        builder.Property(a => a.Source).HasMaxLength(30).IsRequired();
        builder.Property(a => a.ColorHex).HasColumnType("char(7)");

        builder.HasIndex(a => new { a.TenantId, a.ProviderId, a.StartAt });
        builder.HasIndex(a => new { a.TenantId, a.PatientId, a.StartAt });
    }
}

public class AppointmentTypeConfiguration : IEntityTypeConfiguration<AppointmentType>
{
    public void Configure(EntityTypeBuilder<AppointmentType> builder)
    {
        builder.ToTable("appointment_types");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).HasMaxLength(255).IsRequired();
        builder.Property(t => t.ColorHex).HasColumnType("char(7)");
        builder.Property(t => t.DefaultFee).HasColumnType("numeric(10,2)");
        builder.HasIndex(t => new { t.TenantId, t.Name }).IsUnique()
            .HasFilter("\"IsDeleted\" = false");
    }
}

public class AppointmentStatusHistoryConfiguration : IEntityTypeConfiguration<AppointmentStatusHistory>
{
    public void Configure(EntityTypeBuilder<AppointmentStatusHistory> builder)
    {
        builder.ToTable("appointment_status_history");
        builder.HasKey(h => h.Id);

        builder.Property(h => h.AppointmentId).IsRequired();
        builder.Property(h => h.FromStatus).HasMaxLength(30);
        builder.Property(h => h.ToStatus).HasMaxLength(30).IsRequired();
        builder.Property(h => h.Reason).HasMaxLength(500);
        builder.Property(h => h.ChangedAt).HasColumnType("timestamptz").IsRequired();

        builder.HasIndex(h => new { h.TenantId, h.AppointmentId });
    }
}

