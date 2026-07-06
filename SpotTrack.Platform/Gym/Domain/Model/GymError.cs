namespace SpotTrack.Platform.Gyms.Domain.Model;

public enum GymError
{
    GymNotFound,
    InvalidData,
    Forbidden,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
