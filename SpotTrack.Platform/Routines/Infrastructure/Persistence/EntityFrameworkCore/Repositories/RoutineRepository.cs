using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RoutineRepository(AppDbContext context) : BaseRepository<Routine>(context), IRoutineRepository
{
    // Shadow base FindByIdAsync so OwnsMany (ExerciseBlocks) is loaded from its separate table.
    public new async Task<Routine?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => await Context.Set<Routine>()
            .Where(r => r.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<Routine>> FindAllByClientIdAsync(int clientId,
        CancellationToken cancellationToken = default)
        => await Context.Set<Routine>().Where(a => a.ClientId.Value == clientId).ToListAsync(cancellationToken);
}