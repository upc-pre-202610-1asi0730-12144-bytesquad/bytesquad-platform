using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Profiles.Domain.Model.Errors;

public static class ProfileErrors
{
    public static Error ClientNotFound(string message) =>
        new($"{nameof(ProfilesError)}.{nameof(ProfilesError.ClientNotFound)}", message);

    public static Error AdminNotFound(string message) =>
        new($"{nameof(ProfilesError)}.{nameof(ProfilesError.AdminNotFound)}", message);

    public static Error EmailAlreadyRegistered(string message) =>
        new($"{nameof(ProfilesError)}.{nameof(ProfilesError.EmailAlreadyRegistered)}", message);

    public static Error InvalidProfileData(string message) =>
        new($"{nameof(ProfilesError)}.{nameof(ProfilesError.InvalidProfileData)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(ProfilesError)}.{nameof(ProfilesError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(ProfilesError)}.{nameof(ProfilesError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(ProfilesError)}.{nameof(ProfilesError.InternalServerError)}", message);
}
