using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Entities;

namespace SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Sensor>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).ValueGeneratedOnAdd();
            entity.Property(s => s.Type).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(s => s.Identifier).IsRequired().HasMaxLength(100);
            entity.Property(s => s.AdminId).IsRequired();
            entity.Property(s => s.EquipmentId);
            entity.Property(s => s.RegisteredAt).IsRequired();

            entity.OwnsMany(s => s.Captures, capture =>
            {
                capture.ToTable("sensor_captures");
                capture.HasKey(c => c.Id);
                capture.Property(c => c.Id).ValueGeneratedOnAdd();
                capture.Property(c => c.DetectedAt).IsRequired();
                capture.WithOwner().HasForeignKey("sensor_id");
            });
        });

        builder.Entity<SessionTracker>(entity =>
        {
            entity.HasKey(st => st.Id);
            entity.Property(st => st.Id).ValueGeneratedOnAdd();
            entity.Property(st => st.EquipmentId).IsRequired();
            entity.Property(st => st.AdminId).IsRequired();
            entity.Property(st => st.StartedAt).IsRequired();
            entity.Property(st => st.EndedAt);
            entity.Ignore(st => st.IsActive);
            entity.Ignore(st => st.ElapsedSeconds);
        });

        builder.Entity<Anomaly>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();
            entity.Property(a => a.SensorId).IsRequired();
            entity.Property(a => a.AdminId).IsRequired();
            entity.Property(a => a.AnomalyType).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Description).IsRequired().HasMaxLength(500);
            entity.Property(a => a.DetectedAt).IsRequired();
        });
    }
}
