namespace SpotTrack.Platform.Memberships.Domain.Model;

public enum MembershipError
{
    MembershipNotFound,
    InvalidMembershipPeriod,
    InvalidMembershipPlan,
    InvalidMembershipStatus,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
