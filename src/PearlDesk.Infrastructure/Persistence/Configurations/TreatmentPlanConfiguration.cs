using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PearlDesk.Treatments.Domain;

namespace PearlDesk.Infrastructure.Persistence.Configurations;

public class TreatmentPlanConfiguration : IEntityTypeConfiguration<TreatmentPlan>
{
    public void Configure(EntityTypeBuilder<TreatmentPlan> builder)
    {
        builder.ToTable("treatment_plans");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PatientId).IsRequired();
        builder.Property(p => p.Title).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Notes).HasMaxLength(2000);
        builder.Property(p => p.Status).HasMaxLength(50).HasConversion<string>().IsRequired();

        builder.HasIndex(p => new { p.TenantId, p.PatientId });

        builder.HasMany(p => p.Items)
            .WithOne()
            .HasForeignKey(i => i.TreatmentPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TreatmentPlanItemConfiguration : IEntityTypeConfiguration<TreatmentPlanItem>
{
    public void Configure(EntityTypeBuilder<TreatmentPlanItem> builder)
    {
        builder.ToTable("treatment_plan_items");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.TreatmentPlanId).IsRequired();
        builder.Property(i => i.Description).HasMaxLength(500).IsRequired();
        builder.Property(i => i.Surface).HasMaxLength(20);
        builder.Property(i => i.CdtCode).HasMaxLength(20);
        builder.Property(i => i.Fee).HasPrecision(10, 2);
        builder.Property(i => i.Status).HasMaxLength(50).HasConversion<string>().IsRequired();

        builder.HasIndex(i => new { i.TenantId, i.TreatmentPlanId });
    }
}
