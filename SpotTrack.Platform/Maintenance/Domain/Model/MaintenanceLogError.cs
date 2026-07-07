namespace SpotTrack.Platform.Maintenances.Domain.Model;

public enum MaintenanceLogError
{
    TechnicalTicketNotFound,
    TicketNotResolved,
    LogAlreadyExists,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
