namespace SpotTrack.Platform.Maintenances.Domain.Model;

public enum MaintenanceJobError
{
    MaintenanceJobNotFound,
    TechnicalTicketNotFound,
    InvalidMaintenanceJobStatus,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
