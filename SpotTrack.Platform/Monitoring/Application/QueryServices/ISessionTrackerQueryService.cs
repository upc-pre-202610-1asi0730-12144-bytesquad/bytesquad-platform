using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;

namespace SpotTrack.Platform.Monitoring.Application.QueryServices;

public interface ISessionTrackerQueryService
{
    Task<IEnumerable<SessionTracker>> Handle(GetAllSessionTrackersQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<SessionTracker>> Handle(GetSessionTrackersByAdminIdQuery query, CancellationToken cancellationToken);
    Task<SessionTracker?> Handle(GetSessionTrackerByIdQuery query, CancellationToken cancellationToken);
}
