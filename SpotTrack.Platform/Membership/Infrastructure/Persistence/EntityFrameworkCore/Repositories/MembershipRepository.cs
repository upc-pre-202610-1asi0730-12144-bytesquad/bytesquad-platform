using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Memberships.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MembershipRepository(AppDbContext context)
    : BaseRepository<Membership>(context), IMembershipRepository
{
    public async Task<IEnumerable<Membership>> FindAllByClientIdAsync(
        int clientId,
        CancellationToken cancellationToken = default)
        => await Context.Set<Membership>()
            .Where(m => m.ClientId == clientId)
            .ToListAsync(cancellationToken);
}
