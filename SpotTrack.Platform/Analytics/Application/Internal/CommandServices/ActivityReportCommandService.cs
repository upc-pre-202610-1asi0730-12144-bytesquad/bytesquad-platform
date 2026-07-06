using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;
using SpotTrack.Platform.Analytics.Domain.Repositories;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Analytics.Application.Internal.CommandServices;

public class ActivityReportCommandService : IActivityReportCommandService
{
    private readonly IActivityReportRepository _activityReportRepository;

    public ActivityReportCommandService(IActivityReportRepository activityReportRepository)
    {
        _activityReportRepository = activityReportRepository;
    }

    public async Task<Result<ActivityReport>> Handle(RequestActivityAnalysisCommand command)
    {
        var activityReport = new ActivityReport(command);
        await _activityReportRepository.AddAsync(activityReport);
        activityReport.InitializeId();
        await _activityReportRepository.UpdateAsync(activityReport);
        return Result<ActivityReport>.Success(activityReport);
    }

    public async Task<Result<ActivityReport>> Handle(RequestTotalUsageTimeCommand command)
    {
        var activityReportId = new ActivityReportId(command.ActivityReportId);
        var activityReport = await _activityReportRepository.FindByActivityReportIdAsync(activityReportId);

        if (activityReport == null) return Result<ActivityReport>.Failure(AnalyticsError.NotFound, "Activity report not found.");
        if (activityReport.AdminId != command.AuthenticatedAdminId) return Result<ActivityReport>.Failure(AnalyticsError.Forbidden, "You do not have permission to modify this activity report.");

        activityReport.UpdateTotalUsageTime(command.TotalUsageTime);
        await _activityReportRepository.UpdateAsync(activityReport);
        return Result<ActivityReport>.Success(activityReport);
    }

    public async Task<Result<ActivityReport>> Handle(RequestDowntimeCostCommand command)
    {
        var activityReportId = new ActivityReportId(command.ActivityReportId);
        var activityReport = await _activityReportRepository.FindByActivityReportIdAsync(activityReportId);

        if (activityReport == null) return Result<ActivityReport>.Failure(AnalyticsError.NotFound, "Activity report not found.");
        if (activityReport.AdminId != command.AuthenticatedAdminId) return Result<ActivityReport>.Failure(AnalyticsError.Forbidden, "You do not have permission to modify this activity report.");

        activityReport.UpdateDowntimeCost(command.DowntimeCost);
        await _activityReportRepository.UpdateAsync(activityReport);
        return Result<ActivityReport>.Success(activityReport);
    }

    public async Task<Result<ActivityReport>> Handle(RequestPercentageComparisonCommand command)
    {
        var activityReportId = new ActivityReportId(command.ActivityReportId);
        var activityReport = await _activityReportRepository.FindByActivityReportIdAsync(activityReportId);

        if (activityReport == null) return Result<ActivityReport>.Failure(AnalyticsError.NotFound, "Activity report not found.");
        if (activityReport.AdminId != command.AuthenticatedAdminId) return Result<ActivityReport>.Failure(AnalyticsError.Forbidden, "You do not have permission to modify this activity report.");

        activityReport.UpdatePercentageComparison(command.PercentageComparison);
        await _activityReportRepository.UpdateAsync(activityReport);
        return Result<ActivityReport>.Success(activityReport);
    }
}
