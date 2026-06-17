using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Memberships.Domain.Repositories;

public interface IMembershipRepository : IBaseRepository<Membership>
{
    Task<IEnumerable<Membership>> FindAllByClientIdAsync(int clientId,
        CancellationToken cancellationToken = default);
}
