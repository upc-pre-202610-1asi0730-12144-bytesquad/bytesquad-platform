using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Domain.Model.Projections;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Reservations.Domain.Repositories;


public interface IReservationRepository : IBaseRepository<Reservation>
{

    Task<IEnumerable<Reservation>> FindAllByClientIdAsync(int clientId,
        CancellationToken cancellationToken = default);


    Task<IEnumerable<Reservation>> FindAllByEquipmentIdAsync(int equipmentId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<EquipmentUsageStat>> FindEquipmentUsageStatsByIdsAsync(
        IEnumerable<int> equipmentIds,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<HourlyUsageStat>> FindHourlyDistributionByIdsAsync(
        IEnumerable<int> equipmentIds,
        CancellationToken cancellationToken = default);
}