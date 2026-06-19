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

        builder.Entity<TechnicalTicket>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).ValueGeneratedOnAdd();

            entity.Property(t => t.MaintenanceId).IsRequired();
            entity.Property(t => t.EquipmentId).IsRequired();

            entity.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(t => t.AssignedTechnicianId).IsRequired(false);

            entity.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(t => t.MaintenanceProgress)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
        });

        builder.Entity<MaintenanceJob>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Id).ValueGeneratedOnAdd();

            entity.Property(j => j.TechnicalTicketId).IsRequired();
            entity.Property(j => j.TechnicianId).IsRequired();

            entity.Property(j => j.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
        });
    }
}
