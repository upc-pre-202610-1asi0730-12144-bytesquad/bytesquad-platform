using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AdminRepository(AppDbContext context)
    : BaseRepository<Admin>(context), IAdminRepository
{
    public async Task<Admin?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Admin>()
            .FirstOrDefaultAsync(a => a.Email != null && a.Email.Address == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Admin>()
            .AnyAsync(a => a.Email != null && a.Email.Address == email, cancellationToken);
    }
}
