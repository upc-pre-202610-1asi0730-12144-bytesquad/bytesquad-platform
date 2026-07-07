using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Routines.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RoutineSessionRepository(AppDbContext context)
    : BaseRepository<RoutineSession>(context), IRoutineSessionRepository
{
    // Shadow base FindByIdAsync so OwnsMany (CompletedExercises) is loaded from its separate table.
    public new async Task<RoutineSession?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => await Context.Set<RoutineSession>()
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<RoutineSession>> FindAllByClientIdAsync(
        int clientId, CancellationToken cancellationToken = default)
        => await Context.Set<RoutineSession>()
            .Where(s => s.ClientId.Value == clientId)
            .ToListAsync(cancellationToken);
}
