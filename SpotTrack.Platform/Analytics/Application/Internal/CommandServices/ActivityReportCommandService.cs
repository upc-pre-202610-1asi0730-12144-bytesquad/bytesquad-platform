using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Repositories;

namespace SpotTrack.Platform.Analytics.Application.Internal.CommandServices;

public class ActivityReportCommandService : IActivityReportCommandService
{
    private readonly IActivityReportRepository _activityReportRepository;

    // Inyección de dependencias por constructor nativa de C#
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
}