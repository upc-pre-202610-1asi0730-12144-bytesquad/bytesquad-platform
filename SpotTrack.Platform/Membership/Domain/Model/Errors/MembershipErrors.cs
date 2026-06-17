using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Memberships.Domain.Model.Errors;

public static class MembershipErrors
{
    public static Error MembershipNotFound(string message) =>
        new($"{nameof(MembershipError)}.{nameof(MembershipError.MembershipNotFound)}", message);

    public static Error InvalidMembershipPeriod(string message) =>
        new($"{nameof(MembershipError)}.{nameof(MembershipError.InvalidMembershipPeriod)}", message);

    public static Error InvalidMembershipPlan(string message) =>
        new($"{nameof(MembershipError)}.{nameof(MembershipError.InvalidMembershipPlan)}", message);

    public static Error InvalidMembershipStatus(string message) =>
        new($"{nameof(MembershipError)}.{nameof(MembershipError.InvalidMembershipStatus)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(MembershipError)}.{nameof(MembershipError.OperationCancelled)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(MembershipError)}.{nameof(MembershipError.DatabaseError)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(MembershipError)}.{nameof(MembershipError.InternalServerError)}", message);
}
