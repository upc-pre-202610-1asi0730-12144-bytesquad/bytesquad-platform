using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;
using SpotTrack.Platform.Alerts.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository(AppDbContext context)
    : BaseRepository<Alert>(context), IAlertRepository
{
    public async Task<IEnumerable<Alert>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default)
        => await Context.Set<Alert>()
            .Where(a => a.AdminId == adminId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
}
