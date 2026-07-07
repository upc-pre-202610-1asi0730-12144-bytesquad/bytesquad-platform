using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Domain.Repositories;

public interface ISensorRepository : IBaseRepository<Sensor>
{
    Task<IEnumerable<Sensor>> FindAllByEquipmentIdsAsync(IEnumerable<int> equipmentIds,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Sensor>> FindAllOnlineAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Sensor>> FindAllOfflineSinceAsync(DateTimeOffset threshold,
        CancellationToken cancellationToken = default);
}
