using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Domain.Repositories;

public interface IMaintenanceJobRepository : IBaseRepository<MaintenanceJob>
{
    Task<IEnumerable<MaintenanceJob>> FindAllByTechnicianIdAsync(int technicianId,
        CancellationToken cancellationToken = default);
}
