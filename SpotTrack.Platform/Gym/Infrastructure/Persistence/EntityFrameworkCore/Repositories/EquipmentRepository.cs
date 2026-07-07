using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;
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

    public async Task<IEnumerable<Equipment>> FindAvailableAlternativesAsync(
        string equipmentName, int excludeEquipmentId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Equipment>()
            .Where(e => e.Name.Value == equipmentName
                        && e.Status == EquipmentStatus.Available
                        && e.Id != excludeEquipmentId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default)
    {
        var zoneIds = Context.Set<Gym>()
            .Where(g => g.AdminId == adminId)
            .SelectMany(g => g.Branches)
            .SelectMany(b => b.Zones)
            .Select(z => z.Id);

        return await Context.Set<Equipment>()
            .Where(e => zoneIds.Contains(e.ZoneId.Value))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Equipment>> FindAllByGymIdAsync(int gymId, CancellationToken cancellationToken = default)
    {
        var zoneIds = Context.Set<Gym>()
            .Where(g => g.Id == gymId)
            .SelectMany(g => g.Branches)
            .SelectMany(b => b.Zones)
            .Select(z => z.Id);

        return await Context.Set<Equipment>()
            .Where(e => zoneIds.Contains(e.ZoneId.Value))
            .ToListAsync(cancellationToken);
    }
}
