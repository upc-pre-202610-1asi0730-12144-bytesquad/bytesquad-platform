using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Queries;

namespace SpotTrack.Platform.Analytics.Application.QueryServices;

public interface IMaintenanceQuoteQueryService
{
    Task<IEnumerable<MaintenanceQuote>> Handle(GetAllMaintenanceQuotesByAdminIdQuery query, CancellationToken cancellationToken);
}
