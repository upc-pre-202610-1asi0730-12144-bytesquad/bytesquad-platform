using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Memberships.Domain.Repositories;

public interface IBranchAccessRepository : IBaseRepository<BranchAccess>
{
    Task<IEnumerable<BranchAccess>> FindAllByMembershipIdAsync(int membershipId,
        CancellationToken cancellationToken = default);
}
