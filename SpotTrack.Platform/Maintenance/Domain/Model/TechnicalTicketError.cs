namespace SpotTrack.Platform.Maintenances.Domain.Model;

public enum TechnicalTicketError
{
    TechnicalTicketNotFound,
    MaintenanceNotFound,
    InvalidTechnicalTicketStatus,
    Forbidden,
    EquipmentUpdateFailed,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
