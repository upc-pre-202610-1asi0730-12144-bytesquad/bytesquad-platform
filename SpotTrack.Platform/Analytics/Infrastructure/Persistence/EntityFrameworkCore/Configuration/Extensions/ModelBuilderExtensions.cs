using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<ActivityReport>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).ValueGeneratedOnAdd();

            entity.OwnsOne(a => a.ActivityReportId, ownedId =>
            {
                ownedId.WithOwner().HasForeignKey("Id");
                ownedId.Property(x => x.Value).IsRequired().HasColumnName("activity_report_id");
            });

            entity.Property(a => a.TotalUsageTime).IsRequired();
            entity.Property(a => a.DowntimeCost).IsRequired();
            entity.Property(a => a.PercentageComparison).IsRequired();
        });

        builder.Entity<MaintenanceQuote>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).ValueGeneratedOnAdd();

            entity.OwnsOne(m => m.MaintenanceQuoteId, ownedId =>
            {
                ownedId.WithOwner().HasForeignKey("Id");
                ownedId.Property(x => x.Value).IsRequired().HasColumnName("maintenance_quote_id");
            });

            entity.Property(m => m.CorrectiveActionsCost).IsRequired();
            entity.Property(m => m.SparePartsCost).IsRequired();
            entity.Property(m => m.PreventiveCost).IsRequired();
            entity.Property(m => m.TotalMaintenanceCost).IsRequired();
        });

        builder.Entity<ROIProjection>(entity =>
        {
            entity.ToTable("roi_projections");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).ValueGeneratedOnAdd();

            entity.OwnsOne(r => r.RoiProjectionId, ownedId =>
            {
                ownedId.WithOwner().HasForeignKey("Id");
                ownedId.Property(x => x.Value).IsRequired().HasColumnName("roi_projection_id");
            });

            entity.Property(r => r.ProjectedDowntimeCost).IsRequired();
            entity.Property(r => r.ProjectedEarnings).IsRequired();
            entity.Property(r => r.RoiIndex).IsRequired();
            entity.Property(r => r.DemandStatus).IsRequired().HasMaxLength(50);
        });
    }
}
