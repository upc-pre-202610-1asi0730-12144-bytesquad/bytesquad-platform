namespace SpotTrack.Platform.Iam.Domain.Model;

public enum IamError
{
    InvalidCredentials,
    UsernameAlreadyTaken,
    InvalidRole,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
