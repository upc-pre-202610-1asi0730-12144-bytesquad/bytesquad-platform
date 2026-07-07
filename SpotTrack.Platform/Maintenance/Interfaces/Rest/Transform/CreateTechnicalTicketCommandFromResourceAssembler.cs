using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class CreateTechnicalTicketCommandFromResourceAssembler
{
    public static CreateTechnicalTicketCommand ToCommandFromResource(int adminId, CreateTechnicalTicketResource resource) =>
        new(resource.MaintenanceId, resource.EquipmentId, resource.Description, adminId);
}
