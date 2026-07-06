using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Domain.Repositories;

public interface IBusinessRepository : IBaseRepository<Business>
{
    Task<bool> ExistsByAdminIdAsync(int adminId, CancellationToken cancellationToken = default);
}
