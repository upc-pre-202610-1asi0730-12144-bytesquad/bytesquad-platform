using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;
using SpotTrack.Platform.Alerts.Domain.Model.Queries;

namespace SpotTrack.Platform.Alerts.Application.QueryServices;

public interface IAlertQueryService
{
    Task<IEnumerable<Alert>> Handle(GetAlertsByAdminIdQuery query, CancellationToken cancellationToken);
}
