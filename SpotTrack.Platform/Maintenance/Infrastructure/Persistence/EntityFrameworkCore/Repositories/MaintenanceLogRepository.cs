using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Maintenances.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MaintenanceLogRepository(AppDbContext context)
    : BaseRepository<MaintenanceLog>(context), IMaintenanceLogRepository
{
    public async Task<IEnumerable<MaintenanceLog>> FindAllByEquipmentIdAsync(
        int equipmentId,
        CancellationToken cancellationToken = default)
        => await Context.Set<MaintenanceLog>()
            .Where(l => l.EquipmentId == equipmentId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<MaintenanceLog>> FindAllByAdminIdAsync(
        int adminId,
        CancellationToken cancellationToken = default)
        => await Context.Set<MaintenanceLog>()
            .Where(l => l.CompletedByAdminId == adminId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<MaintenanceLog>> FindAllByTicketIdAsync(
        int ticketId,
        CancellationToken cancellationToken = default)
        => await Context.Set<MaintenanceLog>()
            .Where(l => l.TechnicalTicketId == ticketId)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByTechnicalTicketIdAsync(
        int ticketId,
        CancellationToken cancellationToken = default)
        => await Context.Set<MaintenanceLog>()
            .AnyAsync(l => l.TechnicalTicketId == ticketId, cancellationToken);
}
