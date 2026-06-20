namespace SpotTrack.Platform.Maintenances.Domain.Model;

public enum MaintenanceLogError
{
    TechnicalTicketNotFound,
    TicketNotResolved,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
