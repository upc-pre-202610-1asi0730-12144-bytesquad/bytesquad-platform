using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Repositories;

public interface IROIProjectionRepository
{
    Task AddAsync(ROIProjection roiProjection);
    Task UpdateAsync(ROIProjection roiProjection);
    Task<ROIProjection?> FindByRoiProjectionIdAsync(ROIProjectionId roiProjectionId);
}