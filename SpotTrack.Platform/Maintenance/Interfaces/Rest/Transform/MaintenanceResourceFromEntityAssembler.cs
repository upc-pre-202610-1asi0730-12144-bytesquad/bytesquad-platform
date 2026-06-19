using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class MaintenanceResourceFromEntityAssembler
{
    public static MaintenanceResource ToResourceFromEntity(Maintenance maintenance) =>
        new(maintenance.Id,
            maintenance.EquipmentId,
            maintenance.RequestedByAdminId,
            maintenance.Reason,
            maintenance.Status.ToString());
}
