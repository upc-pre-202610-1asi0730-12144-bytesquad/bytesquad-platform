using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;
using SpotTrack.Platform.Maintenances.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Application.Internal.QueryServices;

public class TechnicalTicketQueryService(ITechnicalTicketRepository technicalTicketRepository)
    : ITechnicalTicketQueryService
{
    public async Task<TechnicalTicket?> Handle(GetTechnicalTicketByIdQuery query, CancellationToken cancellationToken)
        => await technicalTicketRepository.FindByIdAsync(query.TechnicalTicketId, cancellationToken);
}
