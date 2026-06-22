using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record MaintenanceStatusUpdateRequestedEvent(
    int TechnicalTicketId,
    int MaintenanceId) : IEvent
{
    public static MaintenanceStatusUpdateRequestedEvent FromTechnicalTicket(TechnicalTicket ticket) =>
        new(ticket.Id, ticket.MaintenanceId);
}
