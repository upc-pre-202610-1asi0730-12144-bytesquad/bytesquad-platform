using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
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
}