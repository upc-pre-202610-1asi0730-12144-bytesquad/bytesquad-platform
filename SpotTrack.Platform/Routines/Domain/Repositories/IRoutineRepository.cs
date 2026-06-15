using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Routines.Domain.Repositories;

public interface IRoutineRepository : IBaseRepository<Routine>
{ 
    Task<IEnumerable<Routine>> FindAllByClientIdAsync(int clientId, CancellationToken cancellationToken = default); 
}