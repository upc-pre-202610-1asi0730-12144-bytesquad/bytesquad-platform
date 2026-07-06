namespace SpotTrack.Platform.Gyms.Domain.Model;

public enum GymError
{
    GymNotFound,
    InvalidData,
    Forbidden,
    BranchLimitExceeded,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
