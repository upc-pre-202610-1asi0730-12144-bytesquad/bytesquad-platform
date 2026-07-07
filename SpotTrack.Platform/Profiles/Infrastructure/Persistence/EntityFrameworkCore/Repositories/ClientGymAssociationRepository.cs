using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Entities;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClientGymAssociationRepository(AppDbContext context)
    : BaseRepository<ClientGymAssociation>(context), IClientGymAssociationRepository
{
    public async Task<bool> ExistsByClientIdAndGymIdAsync(int clientId, int gymId, CancellationToken cancellationToken = default)
        => await Context.Set<ClientGymAssociation>()
            .AnyAsync(a => a.ClientId == clientId && a.GymId == gymId, cancellationToken);

    public async Task<ClientGymAssociation?> FindByClientIdAndGymIdAsync(int clientId, int gymId, CancellationToken cancellationToken = default)
        => await Context.Set<ClientGymAssociation>()
            .FirstOrDefaultAsync(a => a.ClientId == clientId && a.GymId == gymId, cancellationToken);

    public async Task<IEnumerable<ClientGymAssociation>> FindAllByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
        => await Context.Set<ClientGymAssociation>()
            .Where(a => a.ClientId == clientId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ClientGymAssociation>> FindActiveByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
        => await Context.Set<ClientGymAssociation>()
            .Where(a => a.ClientId == clientId && a.Active)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ClientGymAssociation>> FindAllByGymIdAndDniAsync(int gymId, string dni, CancellationToken cancellationToken = default)
        => await Context.Set<ClientGymAssociation>()
            .Where(a => a.GymId == gymId &&
                        Context.Set<Client>().Any(c => c.Id == a.ClientId && c.Dni != null && c.Dni.Value == dni))
            .ToListAsync(cancellationToken);
}
