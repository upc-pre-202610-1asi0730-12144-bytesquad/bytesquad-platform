using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAlertsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Alert>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();
            entity.Property(a => a.AdminId).IsRequired();
            entity.Property(a => a.EquipmentId);
            entity.Property(a => a.Severity).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(a => a.Message).IsRequired().HasMaxLength(500);
            entity.Property(a => a.Resolved).IsRequired();
            entity.Property(a => a.CreatedAt).IsRequired();
        });
    }
}
