using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Domain.Repositories;

public interface ITechnicalTicketRepository : IBaseRepository<TechnicalTicket>
{
    Task<IEnumerable<TechnicalTicket>> FindAllByMaintenanceIdAsync(int maintenanceId,
        CancellationToken cancellationToken = default);
}
