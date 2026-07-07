using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Domain.Repositories;

public interface ISessionTrackerRepository : IBaseRepository<SessionTracker>
{
    Task<IEnumerable<SessionTracker>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default);
}
