using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class GymRepository(AppDbContext context) : BaseRepository<Gym>(context), IGymRepository
{
    public async Task<Gym?> FindByIdWithBranchesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Gym>()
            .Include(g => g.Branches)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<Gym?> FindByIdWithBranchesAndZonesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Gym>()
            .Include(g => g.Branches)
            .ThenInclude(b => b.Zones)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsZoneByIdAsync(int zoneId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Zone>().AnyAsync(z => z.Id == zoneId, cancellationToken);
    }

    public async Task<int?> FindAdminIdByEquipmentIdAsync(
        int equipmentId,
        CancellationToken cancellationToken = default)
    {
        var zoneId = await Context.Set<Equipment>()
            .Where(e => e.Id == equipmentId)
            .Select(e => (int?)e.ZoneId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        if (zoneId is null) return null;

        return await Context.Set<Gym>()
            .Where(g => g.Branches.Any(b => b.Zones.Any(z => z.Id == zoneId.Value)))
            .Select(g => (int?)g.AdminId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsByAdminIdAsync(int adminId, CancellationToken cancellationToken = default) =>
        await Context.Set<Gym>().AnyAsync(g => g.AdminId == adminId, cancellationToken);

    public async Task<Gym?> FindByAdminIdAsync(int adminId, CancellationToken cancellationToken = default) =>
        await Context.Set<Gym>().FirstOrDefaultAsync(g => g.AdminId == adminId, cancellationToken);

    public async Task<IEnumerable<int>> FindAllEquipmentIdsByAdminIdAsync(
        int adminId,
        CancellationToken cancellationToken = default)
    {
        var zoneIds = Context.Set<Gym>()
            .Where(g => g.AdminId == adminId)
            .SelectMany(g => g.Branches)
            .SelectMany(b => b.Zones)
            .Select(z => z.Id);

        return await Context.Set<Equipment>()
            .Where(e => zoneIds.Contains(e.ZoneId.Value))
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);
    }
}
