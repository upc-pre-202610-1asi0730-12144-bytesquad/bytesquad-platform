using Microsoft.EntityFrameworkCore;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Repositories;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using SpotTrack.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace SpotTrack.Platform.Memberships.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BranchAccessRepository(AppDbContext context)
    : BaseRepository<BranchAccess>(context), IBranchAccessRepository
{
    public async Task<IEnumerable<BranchAccess>> FindAllByMembershipIdAsync(
        int membershipId,
        CancellationToken cancellationToken = default)
        => await Context.Set<BranchAccess>()
            .Where(b => b.MembershipId == membershipId)
            .ToListAsync(cancellationToken);
}
