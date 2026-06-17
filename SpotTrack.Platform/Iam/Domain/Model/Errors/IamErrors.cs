using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Iam.Domain.Model.Errors;

public static class IamErrors
{
    public static Error InvalidCredentials(string message) =>
        new($"{nameof(IamError)}.{nameof(IamError.InvalidCredentials)}", message);

    public static Error UsernameAlreadyTaken(string message) =>
        new($"{nameof(IamError)}.{nameof(IamError.UsernameAlreadyTaken)}", message);

    public static Error InvalidRole(string message) =>
        new($"{nameof(IamError)}.{nameof(IamError.InvalidRole)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(IamError)}.{nameof(IamError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(IamError)}.{nameof(IamError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(IamError)}.{nameof(IamError.InternalServerError)}", message);
}
