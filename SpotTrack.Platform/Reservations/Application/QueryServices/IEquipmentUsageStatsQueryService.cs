using SpotTrack.Platform.Reservations.Domain.Model.Projections;
using SpotTrack.Platform.Reservations.Domain.Model.Queries;

namespace SpotTrack.Platform.Reservations.Application.QueryServices;

public interface IEquipmentUsageStatsQueryService
{
    Task<IEnumerable<EquipmentUsageStat>> Handle(GetEquipmentUsageStatsByAdminIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<HourlyUsageStat>> Handle(GetPeakCapacityHoursByAdminIdQuery query, CancellationToken cancellationToken);
}
