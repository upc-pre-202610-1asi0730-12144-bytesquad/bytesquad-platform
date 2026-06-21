using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;
using SpotTrack.Platform.Analytics.Domain.Repositories;

namespace SpotTrack.Platform.Analytics.Application.Internal.CommandServices;

public class ActivityReportCommandService : IActivityReportCommandService
{
    private readonly IActivityReportRepository _activityReportRepository;

    public ActivityReportCommandService(IActivityReportRepository activityReportRepository)
    {
        _activityReportRepository = activityReportRepository;
    }

    public async Task<ActivityReport?> Handle(RequestActivityAnalysisCommand command)
    {
        var activityReport = new ActivityReport(command);
        await _activityReportRepository.AddAsync(activityReport);
        return activityReport;
    }

    public async Task<ActivityReport?> Handle(RequestTotalUsageTimeCommand command)
    {
        var activityReportId = new ActivityReportId(command.ActivityReportId);
        var activityReport = await _activityReportRepository.FindByActivityReportIdAsync(activityReportId);
        
        if (activityReport == null) return null;

        activityReport.UpdateTotalUsageTime(command.TotalUsageTime);
        await _activityReportRepository.UpdateAsync(activityReport); // Usamos un método de actualización
        return activityReport;
    }

    public async Task<ActivityReport?> Handle(RequestDowntimeCostCommand command)
    {
        var activityReportId = new ActivityReportId(command.ActivityReportId);
        var activityReport = await _activityReportRepository.FindByActivityReportIdAsync(activityReportId);
        
        if (activityReport == null) return null;

        activityReport.UpdateDowntimeCost(command.DowntimeCost);
        await _activityReportRepository.UpdateAsync(activityReport);
        return activityReport;
    }
    
    public async Task<ActivityReport?> Handle(RequestPercentageComparisonCommand command)
    {
        var activityReportId = new ActivityReportId(command.ActivityReportId);
        var activityReport = await _activityReportRepository.FindByActivityReportIdAsync(activityReportId);
        
        if (activityReport == null) return null;

        activityReport.UpdatePercentageComparison(command.PercentageComparison);
        await _activityReportRepository.UpdateAsync(activityReport);
        return activityReport;
    }

}