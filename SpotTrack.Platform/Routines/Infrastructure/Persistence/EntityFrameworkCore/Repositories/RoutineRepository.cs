using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RoutineRepository(AppDbContext context) : BaseRepository<Routine>(context), IRoutineRepository
{

    public async Task<IEnumerable<Routine>> FindAllByClientIdAsync(int clientId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<Routine>().Where(a => a.ClientId.Value == clientId).ToListAsync(cancellationToken);
    } 
}