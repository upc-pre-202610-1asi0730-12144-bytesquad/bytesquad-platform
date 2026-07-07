using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;
using SpotTrack.Platform.Alerts.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Alerts.Interfaces.Rest.Transform;

public static class AlertResourceFromEntityAssembler
{
    public static AlertResource ToResourceFromEntity(Alert alert) =>
        new(alert.Id, alert.AdminId, alert.EquipmentId, alert.Severity.ToString(), alert.Message, alert.Resolved, alert.CreatedAt);
}
