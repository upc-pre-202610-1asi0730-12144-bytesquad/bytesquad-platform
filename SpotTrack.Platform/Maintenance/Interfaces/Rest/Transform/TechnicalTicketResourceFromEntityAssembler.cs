using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class TechnicalTicketResourceFromEntityAssembler
{
    public static TechnicalTicketResource ToResourceFromEntity(TechnicalTicket ticket) =>
        new(ticket.Id,
            ticket.MaintenanceId,
            ticket.EquipmentId,
            ticket.Status.ToString(),
            ticket.MaintenanceProgress.ToString(),
            ticket.AssignedTechnicianId,
            ticket.Description);
}
