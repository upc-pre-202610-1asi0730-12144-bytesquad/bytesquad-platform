using SpotTrack.Platform.Alerts.Application.QueryServices;
using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;
using SpotTrack.Platform.Alerts.Domain.Model.Queries;
using SpotTrack.Platform.Alerts.Domain.Repositories;

namespace SpotTrack.Platform.Alerts.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository alertRepository) : IAlertQueryService
{
    public async Task<IEnumerable<Alert>> Handle(GetAlertsByAdminIdQuery query, CancellationToken cancellationToken)
        => await alertRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);
}
