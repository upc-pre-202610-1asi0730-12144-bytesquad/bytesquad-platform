using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Maintenances.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MaintenanceJobRepository(AppDbContext context)
    : BaseRepository<MaintenanceJob>(context), IMaintenanceJobRepository
{
    public async Task<IEnumerable<MaintenanceJob>> FindAllByTechnicianIdAsync(
        int technicianId,
        CancellationToken cancellationToken = default)
        => await Context.Set<MaintenanceJob>()
            .Where(j => j.TechnicianId == technicianId)
            .ToListAsync(cancellationToken);
}
