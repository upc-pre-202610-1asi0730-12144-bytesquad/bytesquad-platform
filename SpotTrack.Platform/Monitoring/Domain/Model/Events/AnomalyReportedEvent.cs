using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Events;

public record AnomalyReportedEvent(int AnomalyId, int SensorId, int AdminId, string AnomalyType, string Description) : IEvent
{
    public static AnomalyReportedEvent FromAnomaly(Anomaly anomaly) =>
        new(anomaly.Id, anomaly.SensorId, anomaly.AdminId, anomaly.AnomalyType, anomaly.Description);
}
