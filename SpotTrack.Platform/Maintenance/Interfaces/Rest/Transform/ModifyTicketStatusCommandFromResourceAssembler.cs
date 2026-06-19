using SpotTrack.Platform.Maintenances.Domain.Model;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class ModifyTicketStatusCommandFromResourceAssembler
{
    public static ModifyTicketStatusCommand ToCommandFromResource(int ticketId, ModifyTicketStatusResource resource) =>
        new(ticketId, Enum.Parse<ETechnicalTicketStatus>(resource.NewStatus, ignoreCase: true));
}
