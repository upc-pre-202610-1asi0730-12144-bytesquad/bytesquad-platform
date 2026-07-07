namespace SpotTrack.Platform.Maintenances.Domain.Model;

public enum MaintenanceError
{
    MaintenanceNotFound,
    InvalidMaintenanceData,
    InvalidMaintenanceStatus,
    Forbidden,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
