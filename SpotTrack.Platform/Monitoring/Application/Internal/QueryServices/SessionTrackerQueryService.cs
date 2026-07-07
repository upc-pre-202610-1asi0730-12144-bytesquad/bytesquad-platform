using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;
using SpotTrack.Platform.Monitoring.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.QueryServices;

public class SessionTrackerQueryService(ISessionTrackerRepository sessionTrackerRepository) : ISessionTrackerQueryService
{
    public async Task<IEnumerable<SessionTracker>> Handle(GetAllSessionTrackersQuery query, CancellationToken cancellationToken)
        => await sessionTrackerRepository.ListAsync(cancellationToken);

    public async Task<IEnumerable<SessionTracker>> Handle(GetSessionTrackersByAdminIdQuery query, CancellationToken cancellationToken)
        => await sessionTrackerRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);

    public async Task<SessionTracker?> Handle(GetSessionTrackerByIdQuery query, CancellationToken cancellationToken)
        => await sessionTrackerRepository.FindByIdAsync(query.Id, cancellationToken);
}
