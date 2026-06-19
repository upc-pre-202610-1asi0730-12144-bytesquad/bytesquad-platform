using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Application.Internal.QueryServices;

public class MaintenanceQueryService(IMaintenanceRepository maintenanceRepository)
    : IMaintenanceQueryService
{
}
