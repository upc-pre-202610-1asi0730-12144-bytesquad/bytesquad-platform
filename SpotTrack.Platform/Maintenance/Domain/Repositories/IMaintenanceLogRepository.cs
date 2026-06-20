using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Domain.Repositories;

public interface IMaintenanceLogRepository : IBaseRepository<MaintenanceLog>
{
    Task<IEnumerable<MaintenanceLog>> FindAllByEquipmentIdAsync(int equipmentId,
        CancellationToken cancellationToken = default);
}
