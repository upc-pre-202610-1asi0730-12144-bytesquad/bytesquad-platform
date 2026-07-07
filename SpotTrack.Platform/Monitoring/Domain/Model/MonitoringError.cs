namespace SpotTrack.Platform.Monitoring.Domain.Model;

public enum MonitoringError
{
    SensorNotFound,
    SessionTrackerNotFound,
    AnomalyReportFailed,
    InvalidSensorData,
    InvalidTrackerData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
