using SpotTrack.Platform.Analytics.Application.QueryServices;
using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Domain.Model.Queries;
using SpotTrack.Platform.Analytics.Domain.Repositories;

namespace SpotTrack.Platform.Analytics.Application.Internal.QueryServices;

public class MaintenanceQuoteQueryService(IMaintenanceQuoteRepository maintenanceQuoteRepository)
    : IMaintenanceQuoteQueryService
{
    public async Task<IEnumerable<MaintenanceQuote>> Handle(
        GetAllMaintenanceQuotesByAdminIdQuery query,
        CancellationToken cancellationToken)
        => await maintenanceQuoteRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);
}
