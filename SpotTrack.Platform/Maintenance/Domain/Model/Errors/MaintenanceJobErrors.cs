using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Errors;

public static class MaintenanceJobErrors
{
    public static Error MaintenanceJobNotFound(string message) =>
        new($"{nameof(MaintenanceJobError)}.{nameof(MaintenanceJobError.MaintenanceJobNotFound)}", message);

    public static Error TechnicalTicketNotFound(string message) =>
        new($"{nameof(MaintenanceJobError)}.{nameof(MaintenanceJobError.TechnicalTicketNotFound)}", message);

    public static Error InvalidMaintenanceJobStatus(string message) =>
        new($"{nameof(MaintenanceJobError)}.{nameof(MaintenanceJobError.InvalidMaintenanceJobStatus)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(MaintenanceJobError)}.{nameof(MaintenanceJobError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(MaintenanceJobError)}.{nameof(MaintenanceJobError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(MaintenanceJobError)}.{nameof(MaintenanceJobError.InternalServerError)}", message);
}
