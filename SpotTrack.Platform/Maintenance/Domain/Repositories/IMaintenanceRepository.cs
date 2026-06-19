using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Domain.Repositories;

public interface IMaintenanceRepository : IBaseRepository<Maintenance>
{
    Task<IEnumerable<Maintenance>> FindAllByEquipmentIdAsync(int equipmentId,
        CancellationToken cancellationToken = default);
}
