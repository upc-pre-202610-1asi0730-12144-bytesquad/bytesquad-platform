using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record MaintenanceRequestedEvent(
    int MaintenanceId,
    int EquipmentId,
    int RequestedByAdminId,
    string Reason) : IEvent
{
    public static MaintenanceRequestedEvent FromMaintenance(Maintenance maintenance) =>
        new(maintenance.Id,
            maintenance.EquipmentId,
            maintenance.RequestedByAdminId,
            maintenance.Reason);
}
