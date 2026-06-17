using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace SpotTrack.Platform.Gyms.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class GymRepository(AppDbContext context) : BaseRepository<Gym>(context), IGymRepository
{
    public override async Task<Gym?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Gym>()
            .Include(g => g.Branches)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }
}
