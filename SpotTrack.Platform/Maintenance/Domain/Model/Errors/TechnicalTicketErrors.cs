using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Errors;

public static class TechnicalTicketErrors
{
    public static Error TechnicalTicketNotFound(string message) =>
        new($"{nameof(TechnicalTicketError)}.{nameof(TechnicalTicketError.TechnicalTicketNotFound)}", message);

    public static Error MaintenanceNotFound(string message) =>
        new($"{nameof(TechnicalTicketError)}.{nameof(TechnicalTicketError.MaintenanceNotFound)}", message);

    public static Error InvalidTechnicalTicketStatus(string message) =>
        new($"{nameof(TechnicalTicketError)}.{nameof(TechnicalTicketError.InvalidTechnicalTicketStatus)}", message);

    public static Error EquipmentUpdateFailed(string message) =>
        new($"{nameof(TechnicalTicketError)}.{nameof(TechnicalTicketError.EquipmentUpdateFailed)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(TechnicalTicketError)}.{nameof(TechnicalTicketError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(TechnicalTicketError)}.{nameof(TechnicalTicketError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(TechnicalTicketError)}.{nameof(TechnicalTicketError.InternalServerError)}", message);
}
