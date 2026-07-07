using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Reservations.Domain.Model;
using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Domain.Model.Projections;
using SpotTrack.Platform.Reservations.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Reservations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;


public class ReservationRepository(AppDbContext context)
    : BaseRepository<Reservation>(context), IReservationRepository
{

    public async Task<IEnumerable<Reservation>> FindAllByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken = default)
        => await Context.Set<Reservation>()
            .Where(r => r.ClientId == clientId)
            .ToListAsync(cancellationToken);


    public async Task<IEnumerable<Reservation>> FindAllByEquipmentIdAsync(
        int equipmentId,
        CancellationToken cancellationToken = default)
        => await Context.Set<Reservation>()
            .Where(r => r.EquipmentId == equipmentId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<EquipmentUsageStat>> FindEquipmentUsageStatsByIdsAsync(
        IEnumerable<int> equipmentIds,
        CancellationToken cancellationToken = default)
    {
        var ids = equipmentIds.ToList();
        // WHERE+projection runs in DB; TimeSpan arithmetic runs in memory (no cross-provider date-diff function needed)
        var rows = await Context.Set<Reservation>()
            .Where(r => r.Status == EReservationStatus.Ended && ids.Contains(r.EquipmentId))
            .Select(r => new { r.EquipmentId, r.StartDate, r.EndDate })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(r => r.EquipmentId)
            .Select(g => new EquipmentUsageStat(
                g.Key,
                g.Sum(r => (r.EndDate - r.StartDate).TotalHours),
                g.Count()));
    }

    public async Task<IEnumerable<HourlyUsageStat>> FindHourlyDistributionByIdsAsync(
        IEnumerable<int> equipmentIds,
        CancellationToken cancellationToken = default)
    {
        var ids = equipmentIds.ToList();
        var rows = await Context.Set<Reservation>()
            .Where(r => (r.Status == EReservationStatus.Ended || r.Status == EReservationStatus.Active)
                        && ids.Contains(r.EquipmentId))
            .Select(r => new { r.StartDate, r.EndDate })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(r => r.StartDate.Hour)
            .Select(g => new HourlyUsageStat(
                g.Key,
                g.Count(),
                g.Sum(r => (r.EndDate - r.StartDate).TotalMinutes)))
            .OrderBy(h => h.Hour);
    }
}