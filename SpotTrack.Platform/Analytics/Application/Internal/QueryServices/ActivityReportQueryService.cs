using SpotTrack.Platform.Analytics.Application.QueryServices;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Queries;
using SpotTrack.Platform.Analytics.Domain.Repositories;

namespace SpotTrack.Platform.Analytics.Application.Internal.QueryServices;

public class ActivityReportQueryService(IActivityReportRepository activityReportRepository)
    : IActivityReportQueryService
{
    public async Task<IEnumerable<ActivityReport>> Handle(
        GetAllActivityReportsByAdminIdQuery query,
        CancellationToken cancellationToken)
        => await activityReportRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);
}
