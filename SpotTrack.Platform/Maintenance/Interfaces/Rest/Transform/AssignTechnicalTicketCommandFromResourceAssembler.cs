using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class AssignTechnicalTicketCommandFromResourceAssembler
{
    public static AssignTechnicalTicketCommand ToCommandFromResource(int ticketId, AssignTechnicalTicketResource resource) =>
        new(ticketId, resource.TechnicianId);
}
