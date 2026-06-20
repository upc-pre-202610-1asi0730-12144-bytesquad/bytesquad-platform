using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record TicketStatusMarkedAsResolvedEvent(
    int TechnicalTicketId,
    int MaintenanceId,
    int EquipmentId) : IEvent
{
    public static TicketStatusMarkedAsResolvedEvent FromTechnicalTicket(TechnicalTicket ticket) =>
        new(ticket.Id, ticket.MaintenanceId, ticket.EquipmentId);
}
