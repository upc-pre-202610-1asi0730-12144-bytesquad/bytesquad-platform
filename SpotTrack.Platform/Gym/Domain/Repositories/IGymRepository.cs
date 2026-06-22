using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Gyms.Domain.Repositories;

public interface IGymRepository : IBaseRepository<Gym>
{
    Task<Gym?> FindByIdWithBranchesAsync(int id, CancellationToken cancellationToken = default);
    Task<Gym?> FindByIdWithBranchesAndZonesAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsZoneByIdAsync(int zoneId, CancellationToken cancellationToken = default);
}
