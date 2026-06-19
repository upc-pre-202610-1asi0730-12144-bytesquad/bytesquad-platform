using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Maintenances.Application.CommandServices;

public interface ITechnicalTicketCommandService
{
    Task<Result<TechnicalTicket>> Handle(CreateTechnicalTicketCommand command, CancellationToken cancellationToken);
    Task<Result<TechnicalTicket>> Handle(AssignTechnicalTicketCommand command, CancellationToken cancellationToken);
}
