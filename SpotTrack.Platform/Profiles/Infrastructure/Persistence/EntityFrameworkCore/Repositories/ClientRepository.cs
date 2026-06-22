using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClientRepository(AppDbContext context)
    : BaseRepository<Client>(context), IClientRepository
{
    public async Task<Client?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Client>()
            .FirstOrDefaultAsync(c => c.Email != null && c.Email.Address == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Client>()
            .AnyAsync(c => c.Email != null && c.Email.Address == email, cancellationToken);
    }
}
