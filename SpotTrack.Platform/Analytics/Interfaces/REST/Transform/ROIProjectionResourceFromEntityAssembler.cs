using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Interfaces.REST.Resources;

namespace SpotTrack.Platform.Analytics.Interfaces.REST.Transform;

public static class ROIProjectionResourceFromEntityAssembler
{
    public static ROIProjectionResource ToResourceFromEntity(ROIProjection entity)
    {
        return new ROIProjectionResource(
            entity.Id,
            entity.RoiProjectionId.Value,
            entity.ProjectedDowntimeCost,
            entity.ProjectedEarnings,
            entity.RoiIndex,
            entity.DemandStatus
        );
    }
}