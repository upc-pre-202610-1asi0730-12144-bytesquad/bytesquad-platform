using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;

namespace SpotTrack.Platform.Maintenances.Application.QueryServices;

public interface ITechnicalTicketQueryService
{
    Task<TechnicalTicket?> Handle(GetTechnicalTicketByIdQuery query, CancellationToken cancellationToken);
}
