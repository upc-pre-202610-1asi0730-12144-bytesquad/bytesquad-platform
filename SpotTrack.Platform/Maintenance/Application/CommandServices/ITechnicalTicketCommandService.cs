using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Maintenances.Application.CommandServices;

public interface ITechnicalTicketCommandService
{
    Task<Result<TechnicalTicket>> Handle(CreateTechnicalTicketCommand command, CancellationToken cancellationToken);
    Task<Result<TechnicalTicket>> Handle(AssignTechnicalTicketCommand command, CancellationToken cancellationToken);
    Task<Result<TechnicalTicket>> Handle(ModifyTicketStatusCommand command, CancellationToken cancellationToken);
    Task<Result<TechnicalTicket>> Handle(RequestUpdateMaintenanceStatusCommand command, CancellationToken cancellationToken);
    Task<Result<TechnicalTicket>> Handle(UpdateMaintenanceStatusCommand command, CancellationToken cancellationToken);
    Task<Result<TechnicalTicket>> Handle(CompleteMaintenanceCommand command, CancellationToken cancellationToken);
}
