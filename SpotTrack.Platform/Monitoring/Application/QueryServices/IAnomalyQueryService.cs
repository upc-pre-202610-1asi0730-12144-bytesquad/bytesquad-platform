using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;

namespace SpotTrack.Platform.Monitoring.Application.QueryServices;

public interface IAnomalyQueryService
{
    Task<IEnumerable<Anomaly>> Handle(GetAnomaliesByAdminIdQuery query, CancellationToken cancellationToken);
}
