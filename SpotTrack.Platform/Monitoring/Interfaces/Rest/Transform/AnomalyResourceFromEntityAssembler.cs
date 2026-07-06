using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;

public static class AnomalyResourceFromEntityAssembler
{
    public static AnomalyResource ToResourceFromEntity(Anomaly anomaly) =>
        new(anomaly.Id, anomaly.ReservationId, anomaly.EquipmentId, anomaly.ZoneId,
            anomaly.AnomalyDescription, anomaly.EmissionDate);
}
