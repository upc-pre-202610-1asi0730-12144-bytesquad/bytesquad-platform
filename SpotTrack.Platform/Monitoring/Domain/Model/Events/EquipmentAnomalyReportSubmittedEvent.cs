using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Events;

/// <summary>
///     Raised when a client reports an equipment anomaly during a reservation.
/// </summary>
/// <remarks>
///     TODO: once the Shared/Alerts bounded context exists, add a handler here that
///     resolves the gym's admin (via IGymContextFacade) and creates an Alert for them,
///     mirroring the reference's AnomalyReportSubmittedAlertEventHandler.
/// </remarks>
public record EquipmentAnomalyReportSubmittedEvent(
    int AnomalyId,
    int ReservationId,
    int EquipmentId,
    int ZoneId,
    string AnomalyDescription,
    DateTimeOffset EmissionDate) : IEvent
{
    public static EquipmentAnomalyReportSubmittedEvent FromAnomaly(Anomaly anomaly) =>
        new(anomaly.Id,
            anomaly.ReservationId,
            anomaly.EquipmentId,
            anomaly.ZoneId,
            anomaly.AnomalyDescription,
            anomaly.EmissionDate);
}
