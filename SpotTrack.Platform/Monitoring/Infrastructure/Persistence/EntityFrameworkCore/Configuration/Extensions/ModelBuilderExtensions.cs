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

        builder.Entity<Sensor>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();
            entity.ToTable("sensors");

            entity.Property(s => s.EquipmentId).IsRequired().HasColumnName("equipment_id");
            entity.Property(s => s.MacAddress).IsRequired().HasMaxLength(50).HasColumnName("mac_address");
            entity.Property(s => s.Location).IsRequired().HasMaxLength(100).HasColumnName("location");
            entity.Property(s => s.Status).IsRequired().HasConversion<string>().HasMaxLength(20).HasColumnName("status");
            entity.Property(s => s.BatteryLevel).IsRequired().HasColumnName("battery_level");
            entity.Property(s => s.SignalStrength).IsRequired().HasColumnName("signal_strength");
            entity.Property(s => s.FirmwareVersion).IsRequired().HasMaxLength(20).HasColumnName("firmware_version");
            entity.Property(s => s.LastHeartbeat).IsRequired().HasColumnName("last_heartbeat");
            entity.Property(s => s.LastStatusChangeAt).IsRequired().HasColumnName("last_status_change_at");
        });
    }
}
