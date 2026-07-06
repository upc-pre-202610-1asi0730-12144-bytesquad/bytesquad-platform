using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Analytics.Application.CommandServices;

public interface IActivityReportCommandService
{
    Task<Result<ActivityReport>> Handle(RequestActivityAnalysisCommand command);

    Task<Result<ActivityReport>> Handle(RequestTotalUsageTimeCommand command);

    Task<Result<ActivityReport>> Handle(RequestDowntimeCostCommand command);

    Task<Result<ActivityReport>> Handle(RequestPercentageComparisonCommand command);
}
