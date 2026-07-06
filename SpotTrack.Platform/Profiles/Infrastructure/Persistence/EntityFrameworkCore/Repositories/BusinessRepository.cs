using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BusinessRepository(AppDbContext context) : BaseRepository<Business>(context), IBusinessRepository
{
    public async Task<bool> ExistsByAdminIdAsync(int adminId, CancellationToken cancellationToken = default) =>
        await Context.Set<Business>().AnyAsync(b => b.AdminId == adminId, cancellationToken);
}
