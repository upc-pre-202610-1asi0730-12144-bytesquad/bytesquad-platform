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
}
