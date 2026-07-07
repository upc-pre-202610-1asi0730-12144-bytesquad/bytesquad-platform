using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SensorRepository(AppDbContext context)
    : BaseRepository<Sensor>(context), ISensorRepository
{
    public new async Task<Sensor?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => await Context.Set<Sensor>()
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<Sensor>> FindAllBySensorTypeAsync(SensorType type, CancellationToken cancellationToken = default)
        => await Context.Set<Sensor>()
            .Where(s => s.Type == type)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Sensor>> FindAllByAdminIdAndTypeAsync(int adminId, SensorType type, CancellationToken cancellationToken = default)
        => await Context.Set<Sensor>()
            .Where(s => s.AdminId == adminId && s.Type == type)
            .ToListAsync(cancellationToken);
}
