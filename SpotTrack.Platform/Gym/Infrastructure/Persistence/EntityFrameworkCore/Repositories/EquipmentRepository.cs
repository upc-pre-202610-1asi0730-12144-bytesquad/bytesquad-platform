using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class EquipmentRepository(AppDbContext context) : BaseRepository<Equipment>(context), IEquipmentRepository
{
    public async Task<IEnumerable<Equipment>> FindAllByZoneIdsAsync(
        IEnumerable<int> zoneIds, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Equipment>()
            .Where(e => zoneIds.Contains(e.ZoneId.Value))
            .ToListAsync(cancellationToken);
    }
}
