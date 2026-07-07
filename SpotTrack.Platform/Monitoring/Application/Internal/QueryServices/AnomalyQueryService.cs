using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;
using SpotTrack.Platform.Monitoring.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.QueryServices;

public class AnomalyQueryService(IAnomalyRepository anomalyRepository) : IAnomalyQueryService
{
    public async Task<IEnumerable<Anomaly>> Handle(GetAnomaliesByAdminIdQuery query, CancellationToken cancellationToken)
        => await anomalyRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);
}
