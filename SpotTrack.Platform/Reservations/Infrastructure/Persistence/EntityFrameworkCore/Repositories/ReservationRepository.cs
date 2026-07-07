using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
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
        return await Context.Set<Reservation>()
            .Where(r => r.Status == EReservationStatus.Ended && ids.Contains(r.EquipmentId))
            .GroupBy(r => r.EquipmentId)
            .Select(g => new EquipmentUsageStat(
                g.Key,
                g.Sum(r => EF.Functions.DateDiffMinute(r.StartDate, r.EndDate) / 60.0),
                g.Count()))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<HourlyUsageStat>> FindHourlyDistributionByIdsAsync(
        IEnumerable<int> equipmentIds,
        CancellationToken cancellationToken = default)
    {
        var ids = equipmentIds.ToList();
        return await Context.Set<Reservation>()
            .Where(r => (r.Status == EReservationStatus.Ended || r.Status == EReservationStatus.Active)
                        && ids.Contains(r.EquipmentId))
            .GroupBy(r => r.StartDate.Hour)
            .Select(g => new HourlyUsageStat(
                g.Key,
                g.Count(),
                g.Sum(r => EF.Functions.DateDiffMinute(r.StartDate, r.EndDate))))
            .OrderBy(h => h.Hour)
            .ToListAsync(cancellationToken);
    }
}