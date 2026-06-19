using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class AcceptMaintenanceCommandFromResourceAssembler
{
    public static CreateAcceptMaintenanceCommand ToCommandFromResource(AcceptMaintenanceResource resource) =>
        new(resource.TechnicalTicketId, resource.TechnicianId);
}
