using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Errors;

public static class MaintenanceLogErrors
{
    public static Error TechnicalTicketNotFound(string message) =>
        new($"{nameof(MaintenanceLogError)}.{nameof(MaintenanceLogError.TechnicalTicketNotFound)}", message);

    public static Error TicketNotResolved(string message) =>
        new($"{nameof(MaintenanceLogError)}.{nameof(MaintenanceLogError.TicketNotResolved)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(MaintenanceLogError)}.{nameof(MaintenanceLogError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(MaintenanceLogError)}.{nameof(MaintenanceLogError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(MaintenanceLogError)}.{nameof(MaintenanceLogError.InternalServerError)}", message);
}
