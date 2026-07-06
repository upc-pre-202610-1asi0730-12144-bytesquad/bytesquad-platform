using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Analytics.Application.CommandServices;

public interface IROIProjectionCommandService
{
    Task<Result<ROIProjection>> Handle(RequestDowntimeCostProjectionCommand command);

    Task<Result<ROIProjection>> Handle(RequestEarningsProjectionCommand command);

    Task<Result<ROIProjection>> Handle(RequestROICommand command);
}
