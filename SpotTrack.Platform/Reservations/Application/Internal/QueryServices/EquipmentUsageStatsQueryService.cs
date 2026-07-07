using SpotTrack.Platform.Gyms.Interfaces.Acl;
using SpotTrack.Platform.Reservations.Application.QueryServices;
using SpotTrack.Platform.Reservations.Domain.Model.Projections;
using SpotTrack.Platform.Reservations.Domain.Model.Queries;
using SpotTrack.Platform.Reservations.Domain.Repositories;

namespace SpotTrack.Platform.Reservations.Application.Internal.QueryServices;

public class EquipmentUsageStatsQueryService(
    IReservationRepository reservationRepository,
    IGymContextFacade gymContextFacade) : IEquipmentUsageStatsQueryService
{
    public async Task<IEnumerable<EquipmentUsageStat>> Handle(
        GetEquipmentUsageStatsByAdminIdQuery query,
        CancellationToken cancellationToken)
    {
        var equipmentIds = await gymContextFacade.GetEquipmentIdsByAdminIdAsync(query.AdminId, cancellationToken);
        return await reservationRepository.FindEquipmentUsageStatsByIdsAsync(equipmentIds, cancellationToken);
    }

    public async Task<IEnumerable<HourlyUsageStat>> Handle(
        GetPeakCapacityHoursByAdminIdQuery query,
        CancellationToken cancellationToken)
    {
        var equipmentIds = await gymContextFacade.GetEquipmentIdsByAdminIdAsync(query.AdminId, cancellationToken);
        return await reservationRepository.FindHourlyDistributionByIdsAsync(equipmentIds, cancellationToken);
    }
}
