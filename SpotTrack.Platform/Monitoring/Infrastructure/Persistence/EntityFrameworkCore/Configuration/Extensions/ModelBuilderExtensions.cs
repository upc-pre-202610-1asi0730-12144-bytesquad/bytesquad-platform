using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Anomaly>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();
            entity.ToTable("anomalies");

            entity.Property(a => a.ReservationId).IsRequired().HasColumnName("reservation_id");
            entity.Property(a => a.EquipmentId).IsRequired().HasColumnName("equipment_id");
            entity.Property(a => a.ZoneId).IsRequired().HasColumnName("zone_id");
            entity.Property(a => a.AnomalyDescription).IsRequired().HasMaxLength(500).HasColumnName("anomaly_description");
            entity.Property(a => a.EmissionDate).IsRequired().HasColumnName("emission_date");
        });
    }
}
