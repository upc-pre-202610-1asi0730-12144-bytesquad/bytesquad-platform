using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Repositories;

public interface IActivityReportRepository
{
    Task AddAsync(ActivityReport activityReport);
    Task<ActivityReport?> FindByActivityReportIdAsync(ActivityReportId activityReportId);
    Task UpdateAsync(ActivityReport activityReport);
}