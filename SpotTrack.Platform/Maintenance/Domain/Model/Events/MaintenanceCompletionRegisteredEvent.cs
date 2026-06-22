using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record MaintenanceCompletionRegisteredEvent(
    int MaintenanceLogId,
    int TechnicalTicketId,
    int EquipmentId,
    int CompletedByAdminId) : IEvent
{
    public static MaintenanceCompletionRegisteredEvent FromMaintenanceLog(MaintenanceLog log) =>
        new(log.Id,
            log.TechnicalTicketId,
            log.EquipmentId,
            log.CompletedByAdminId);
}
