using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Domain.Repositories;

public interface IAnomalyRepository : IBaseRepository<Anomaly>
{
}
