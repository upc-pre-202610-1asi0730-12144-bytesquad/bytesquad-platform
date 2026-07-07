using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Domain.Repositories;

public interface ISensorRepository : IBaseRepository<Sensor>
{
    Task<IEnumerable<Sensor>> FindAllBySensorTypeAsync(SensorType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Sensor>> FindAllByAdminIdAndTypeAsync(int adminId, SensorType type, CancellationToken cancellationToken = default);
}
