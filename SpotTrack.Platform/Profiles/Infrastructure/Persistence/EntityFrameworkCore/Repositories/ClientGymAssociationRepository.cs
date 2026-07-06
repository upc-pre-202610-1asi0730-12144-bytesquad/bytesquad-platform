using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Profiles.Domain.Model.Entities;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClientGymAssociationRepository(AppDbContext context)
    : BaseRepository<ClientGymAssociation>(context), IClientGymAssociationRepository
{
    public async Task<bool> ExistsByClientIdAndGymIdAsync(int clientId, int gymId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClientGymAssociation>()
            .AnyAsync(a => a.ClientId == clientId && a.GymId == gymId, cancellationToken);
    }

    public async Task<ClientGymAssociation?> FindByClientIdAndGymIdAsync(int clientId, int gymId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClientGymAssociation>()
            .FirstOrDefaultAsync(a => a.ClientId == clientId && a.GymId == gymId, cancellationToken);
    }

    public async Task<IEnumerable<ClientGymAssociation>> FindAllByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClientGymAssociation>()
            .Where(a => a.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClientGymAssociation>> FindAllByClientIdAndActiveAsync(int clientId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<ClientGymAssociation>()
            .Where(a => a.ClientId == clientId && a.Active)
            .ToListAsync(cancellationToken);
    }
}
