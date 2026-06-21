using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;

namespace SpotTrack.Platform.Analytics.Application.CommandServices;

public interface IActivityReportCommandService
{
    Task<ActivityReport?> Handle(RequestActivityAnalysisCommand command);
    
    Task<ActivityReport?> Handle(RequestTotalUsageTimeCommand command);
    
    Task<ActivityReport?> Handle(RequestDowntimeCostCommand command);

    
}