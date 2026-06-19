namespace SpotTrack.Platform.Maintenances.Domain.Model;

public enum TechnicalTicketError
{
    TechnicalTicketNotFound,
    MaintenanceNotFound,
    InvalidTechnicalTicketStatus,
    EquipmentUpdateFailed,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
