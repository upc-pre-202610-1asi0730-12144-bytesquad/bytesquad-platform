using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Maintenances.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MaintenanceRepository(AppDbContext context)
    : BaseRepository<Maintenance>(context), IMaintenanceRepository
{
    public async Task<IEnumerable<Maintenance>> FindAllByEquipmentIdAsync(
        int equipmentId,
        CancellationToken cancellationToken = default)
        => await Context.Set<Maintenance>()
            .Where(m => m.EquipmentId == equipmentId)
            .ToListAsync(cancellationToken);
}
