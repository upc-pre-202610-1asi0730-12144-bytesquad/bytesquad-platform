using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class MaintenanceLogResourceFromEntityAssembler
{
    public static MaintenanceLogResource ToResourceFromEntity(MaintenanceLog log) =>
        new(log.Id,
            log.TechnicalTicketId,
            log.EquipmentId,
            log.CompletedByAdminId,
            log.CompletedAt,
            log.Notes);
}
