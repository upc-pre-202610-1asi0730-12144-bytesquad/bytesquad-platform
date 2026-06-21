using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;

namespace SpotTrack.Platform.Analytics.Application.CommandServices;

public interface IROIProjectionCommandService
{
    Task<ROIProjection?> Handle(RequestDowntimeCostProjectionCommand command);
}