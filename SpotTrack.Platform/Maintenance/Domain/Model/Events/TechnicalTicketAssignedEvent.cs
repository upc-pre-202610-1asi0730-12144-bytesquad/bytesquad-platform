using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record TechnicalTicketAssignedEvent(
    int TechnicalTicketId,
    int EquipmentId,
    int AssignedTechnicianId) : IEvent
{
    public static TechnicalTicketAssignedEvent FromTechnicalTicket(TechnicalTicket ticket) =>
        new(ticket.Id,
            ticket.EquipmentId,
            ticket.AssignedTechnicianId!.Value);
}
