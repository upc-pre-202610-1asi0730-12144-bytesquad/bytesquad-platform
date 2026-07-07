using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AnomalyRepository(AppDbContext context)
    : BaseRepository<Anomaly>(context), IAnomalyRepository
{
    public async Task<IEnumerable<Anomaly>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default)
        => await Context.Set<Anomaly>()
            .Where(a => a.AdminId == adminId)
            .ToListAsync(cancellationToken);
}
