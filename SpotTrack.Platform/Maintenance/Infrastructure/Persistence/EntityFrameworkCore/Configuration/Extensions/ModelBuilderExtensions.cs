using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Maintenances.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMaintenanceConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).ValueGeneratedOnAdd();

            entity.Property(m => m.EquipmentId).IsRequired();
            entity.Property(m => m.RequestedByAdminId).IsRequired();

            entity.Property(m => m.Reason)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
        });
    }
}
