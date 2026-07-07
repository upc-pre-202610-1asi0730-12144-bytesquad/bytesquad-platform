using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;
using SpotTrack.Platform.Maintenances.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Application.Internal.QueryServices;

public class MaintenanceLogQueryService(IMaintenanceLogRepository maintenanceLogRepository)
    : IMaintenanceLogQueryService
{
    public async Task<IEnumerable<MaintenanceLog>> Handle(
        GetAllMaintenanceLogsByAdminIdQuery query,
        CancellationToken cancellationToken = default)
        => await maintenanceLogRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);

    public async Task<IEnumerable<MaintenanceLog>> Handle(
        GetMaintenanceLogsByTicketIdQuery query,
        CancellationToken cancellationToken = default)
        => await maintenanceLogRepository.FindAllByTicketIdAsync(query.TicketId, cancellationToken);
}
