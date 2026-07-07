using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;

public static class AnomalyResourceFromEntityAssembler
{
    public static AnomalyResource ToResourceFromEntity(Anomaly anomaly) =>
        new(anomaly.Id, anomaly.SensorId, anomaly.AdminId, anomaly.AnomalyType, anomaly.Description, anomaly.DetectedAt);
}
