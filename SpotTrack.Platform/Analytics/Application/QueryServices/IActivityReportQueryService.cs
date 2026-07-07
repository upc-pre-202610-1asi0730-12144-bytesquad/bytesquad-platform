using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Queries;

namespace SpotTrack.Platform.Analytics.Application.QueryServices;

public interface IActivityReportQueryService
{
    Task<IEnumerable<ActivityReport>> Handle(GetAllActivityReportsByAdminIdQuery query, CancellationToken cancellationToken);
}
