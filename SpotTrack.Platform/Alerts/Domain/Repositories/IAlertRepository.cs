using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Alerts.Domain.Repositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<IEnumerable<Alert>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default);
}
