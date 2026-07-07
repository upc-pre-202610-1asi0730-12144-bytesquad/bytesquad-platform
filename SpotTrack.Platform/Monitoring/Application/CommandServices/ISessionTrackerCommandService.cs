using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Monitoring.Application.CommandServices;

public interface ISessionTrackerCommandService
{
    Task<Result<SessionTracker>> Handle(CreateSessionTrackerCommand command, CancellationToken cancellationToken);
    Task<Result<SessionTracker>> Handle(EndSessionTrackerCommand command, CancellationToken cancellationToken);
}
