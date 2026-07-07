using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Maintenances.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TechnicalTicketRepository(AppDbContext context)
    : BaseRepository<TechnicalTicket>(context), ITechnicalTicketRepository
{
    public async Task<IEnumerable<TechnicalTicket>> FindAllByMaintenanceIdAsync(
        int maintenanceId,
        CancellationToken cancellationToken = default)
        => await Context.Set<TechnicalTicket>()
            .Where(t => t.MaintenanceId == maintenanceId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<TechnicalTicket>> FindAllByAdminIdAsync(
        int adminId,
        CancellationToken cancellationToken = default)
        => await (
            from t in Context.Set<TechnicalTicket>()
            join m in Context.Set<Maintenance>() on t.MaintenanceId equals m.Id
            where m.RequestedByAdminId == adminId
            select t
        ).ToListAsync(cancellationToken);
}
