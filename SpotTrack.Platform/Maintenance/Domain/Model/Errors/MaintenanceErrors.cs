using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Errors;

public static class MaintenanceErrors
{
    public static Error MaintenanceNotFound(string message) =>
        new($"{nameof(MaintenanceError)}.{nameof(MaintenanceError.MaintenanceNotFound)}", message);

    public static Error InvalidMaintenanceData(string message) =>
        new($"{nameof(MaintenanceError)}.{nameof(MaintenanceError.InvalidMaintenanceData)}", message);

    public static Error InvalidMaintenanceStatus(string message) =>
        new($"{nameof(MaintenanceError)}.{nameof(MaintenanceError.InvalidMaintenanceStatus)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(MaintenanceError)}.{nameof(MaintenanceError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(MaintenanceError)}.{nameof(MaintenanceError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(MaintenanceError)}.{nameof(MaintenanceError.InternalServerError)}", message);
}
