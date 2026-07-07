using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SessionTrackerRepository(AppDbContext context)
    : BaseRepository<SessionTracker>(context), ISessionTrackerRepository
{
    public async Task<IEnumerable<SessionTracker>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default)
        => await Context.Set<SessionTracker>()
            .Where(st => st.AdminId == adminId)
            .ToListAsync(cancellationToken);
}
