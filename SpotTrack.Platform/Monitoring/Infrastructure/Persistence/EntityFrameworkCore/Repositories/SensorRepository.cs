using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SensorRepository(AppDbContext context) : BaseRepository<Sensor>(context), ISensorRepository
{
    public async Task<IEnumerable<Sensor>> FindAllByEquipmentIdsAsync(IEnumerable<int> equipmentIds,
        CancellationToken cancellationToken = default)
        => await Context.Set<Sensor>()
            .Where(s => equipmentIds.Contains(s.EquipmentId))
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Sensor>> FindAllOnlineAsync(CancellationToken cancellationToken = default)
        => await Context.Set<Sensor>()
            .Where(s => s.Status == ESensorStatus.Online)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Sensor>> FindAllOfflineSinceAsync(DateTimeOffset threshold,
        CancellationToken cancellationToken = default)
        => await Context.Set<Sensor>()
            .Where(s => s.Status == ESensorStatus.Offline && s.LastStatusChangeAt <= threshold)
            .ToListAsync(cancellationToken);
}
