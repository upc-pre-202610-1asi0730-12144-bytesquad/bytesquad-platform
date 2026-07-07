namespace SpotTrack.Platform.Monitoring.Domain.Model;

public enum MonitoringError
{
    InvalidAnomalyData,
    OperationCancelled,
    DatabaseError,
    InternalServerError,
    InvalidSensorData,
    SensorNotFound,
    InvalidSensorStatus
}
