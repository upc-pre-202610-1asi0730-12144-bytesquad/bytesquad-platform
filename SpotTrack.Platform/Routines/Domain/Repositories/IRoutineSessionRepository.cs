using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Routines.Domain.Repositories;

public interface IRoutineSessionRepository : IBaseRepository<RoutineSession>
{
    Task<IEnumerable<RoutineSession>> FindAllByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
}
