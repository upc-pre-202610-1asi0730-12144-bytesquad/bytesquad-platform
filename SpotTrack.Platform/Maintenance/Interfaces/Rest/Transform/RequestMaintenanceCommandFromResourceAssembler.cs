using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class RequestMaintenanceCommandFromResourceAssembler
{
    public static CreateRequestMaintenanceCommand ToCommandFromResource(int adminId, RequestMaintenanceResource resource) =>
        new(resource.EquipmentId, adminId, resource.Reason);
}
