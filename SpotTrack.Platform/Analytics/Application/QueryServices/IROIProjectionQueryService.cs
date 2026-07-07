using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Queries;

namespace SpotTrack.Platform.Analytics.Application.QueryServices;

public interface IROIProjectionQueryService
{
    Task<IEnumerable<ROIProjection>> Handle(GetAllROIProjectionsByAdminIdQuery query, CancellationToken cancellationToken);
}
