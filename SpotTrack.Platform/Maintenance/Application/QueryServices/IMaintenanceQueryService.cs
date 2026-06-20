using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;

namespace SpotTrack.Platform.Maintenances.Application.QueryServices;

public interface IMaintenanceQueryService
{
    Task<IEnumerable<Maintenance>> Handle(GetAllMaintenancesByEquipmentIdQuery query, CancellationToken cancellationToken);
}
