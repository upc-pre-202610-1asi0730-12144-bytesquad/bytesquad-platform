using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Errors;

public static class TechnicianErrors
{
    public static Error TechnicianNotFound(string message) =>
        new($"{nameof(TechnicianError)}.{nameof(TechnicianError.TechnicianNotFound)}", message);

    public static Error InvalidTechnicianData(string message) =>
        new($"{nameof(TechnicianError)}.{nameof(TechnicianError.InvalidTechnicianData)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(TechnicianError)}.{nameof(TechnicianError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(TechnicianError)}.{nameof(TechnicianError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(TechnicianError)}.{nameof(TechnicianError.InternalServerError)}", message);
}
