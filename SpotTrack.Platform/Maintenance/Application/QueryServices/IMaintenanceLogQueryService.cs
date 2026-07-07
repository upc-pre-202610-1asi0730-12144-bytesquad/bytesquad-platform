using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;

namespace SpotTrack.Platform.Maintenances.Application.QueryServices;

public interface IMaintenanceLogQueryService
{
    Task<IEnumerable<MaintenanceLog>> Handle(GetAllMaintenanceLogsByAdminIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaintenanceLog>> Handle(GetMaintenanceLogsByTicketIdQuery query, CancellationToken cancellationToken = default);
}
