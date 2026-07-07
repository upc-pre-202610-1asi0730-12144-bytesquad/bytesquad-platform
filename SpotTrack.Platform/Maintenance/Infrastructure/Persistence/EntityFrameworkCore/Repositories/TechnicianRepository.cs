using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Maintenances.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TechnicianRepository(AppDbContext context)
    : BaseRepository<Technician>(context), ITechnicianRepository
{
    public async Task<IEnumerable<Technician>> FindAllByAdminIdAsync(
        int adminId,
        CancellationToken cancellationToken = default)
        => await Context.Set<Technician>()
            .Where(t => t.AdminId == adminId)
            .ToListAsync(cancellationToken);
}
