using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;
using SpotTrack.Platform.Maintenances.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Application.Internal.QueryServices;

public class MaintenanceQueryService(IMaintenanceRepository maintenanceRepository)
    : IMaintenanceQueryService
{
    public async Task<IEnumerable<Maintenance>> Handle(
        GetAllMaintenancesByEquipmentIdQuery query,
        CancellationToken cancellationToken)
        => await maintenanceRepository.FindAllByEquipmentIdAsync(query.EquipmentId, cancellationToken);
}
