namespace SpotTrack.Platform.Profiles.Domain.Model;

public enum ProfilesError
{
    ClientNotFound,
    AdminNotFound,
    EmailAlreadyRegistered,
    InvalidProfileData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
