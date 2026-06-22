namespace SpotTrack.Platform.Memberships.Domain.Model;

public enum BranchAccessError
{
    BranchAccessNotFound,
    MembershipNotFound,
    DatabaseError,
    OperationCancelled,
    InternalServerError
}
