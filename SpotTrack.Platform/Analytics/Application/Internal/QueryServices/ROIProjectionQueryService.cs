using SpotTrack.Platform.Analytics.Application.QueryServices;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Queries;
using SpotTrack.Platform.Analytics.Domain.Repositories;

namespace SpotTrack.Platform.Analytics.Application.Internal.QueryServices;

public class ROIProjectionQueryService(IROIProjectionRepository roiProjectionRepository)
    : IROIProjectionQueryService
{
    public async Task<IEnumerable<ROIProjection>> Handle(
        GetAllROIProjectionsByAdminIdQuery query,
        CancellationToken cancellationToken)
        => await roiProjectionRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);
}
