using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RoutineSessionRepository(AppDbContext context)
    : BaseRepository<RoutineSession>(context), IRoutineSessionRepository
{
    public async Task<IEnumerable<RoutineSession>> FindAllByClientIdAsync(
        int clientId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<RoutineSession>()
            .Where(s => s.ClientId.Value == clientId)
            .ToListAsync(cancellationToken);
    }
}
