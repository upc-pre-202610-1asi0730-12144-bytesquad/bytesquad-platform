using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Interfaces.REST.Resources;

namespace SpotTrack.Platform.Analytics.Interfaces.REST.Transform;

public static class ActivityReportResourceFromEntityAssembler
{
    public static ActivityReportResource ToResourceFromEntity(ActivityReport entity)
    {
        return new ActivityReportResource(
            entity.Id,
            entity.ActivityReportId.Value,
            entity.TotalUsageTime,
            entity.DowntimeCost,
            entity.PercentageComparison
        );
    }
}