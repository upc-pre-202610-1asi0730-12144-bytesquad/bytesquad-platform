using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Gym.Domain.Model.Errors;

public static class GymErrors
{
    public static Error InvalidData(string message) =>
        new($"{nameof(GymError)}.{nameof(GymError.InvalidData)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(GymError)}.{nameof(GymError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(GymError)}.{nameof(GymError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(GymError)}.{nameof(GymError.InternalServerError)}", message);
}
