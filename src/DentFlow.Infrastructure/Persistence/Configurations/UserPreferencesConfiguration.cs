using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DentFlow.Domain.Identity;

namespace DentFlow.Infrastructure.Persistence.Configurations;

public class UserPreferencesConfiguration : IEntityTypeConfiguration<UserPreferences>
{
    public void Configure(EntityTypeBuilder<UserPreferences> builder)
    {
        builder.ToTable("user_preferences");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Theme).HasMaxLength(20).IsRequired();
        builder.Property(p => p.Language).HasMaxLength(10).IsRequired();
        builder.Property(p => p.TimeFormat).HasMaxLength(10).IsRequired();
        builder.Property(p => p.DefaultCalendarView).HasMaxLength(20).IsRequired();

        builder.HasIndex(p => p.UserId).IsUnique();

        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<UserPreferences>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
