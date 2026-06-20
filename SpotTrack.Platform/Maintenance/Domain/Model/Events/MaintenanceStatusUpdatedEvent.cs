using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record MaintenanceStatusUpdatedEvent(
    int TechnicalTicketId,
    int MaintenanceId,
    EMaintenanceProgress NewProgress) : IEvent
{
    public static MaintenanceStatusUpdatedEvent FromTechnicalTicket(TechnicalTicket ticket) =>
        new(ticket.Id, ticket.MaintenanceId, ticket.MaintenanceProgress);
}
