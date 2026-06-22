using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record TechnicalTicketCreatedEvent(
    int TechnicalTicketId,
    int MaintenanceId,
    int EquipmentId,
    string Description) : IEvent
{
    public static TechnicalTicketCreatedEvent FromTechnicalTicket(TechnicalTicket ticket) =>
        new(ticket.Id,
            ticket.MaintenanceId,
            ticket.EquipmentId,
            ticket.Description);
}
