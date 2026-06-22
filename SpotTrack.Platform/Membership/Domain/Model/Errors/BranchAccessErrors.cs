using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Memberships.Domain.Model.Errors;

public static class BranchAccessErrors
{
    public static Error BranchAccessNotFound(string message) =>
        new($"{nameof(BranchAccessError)}.{nameof(BranchAccessError.BranchAccessNotFound)}", message);

    public static Error MembershipNotFound(string message) =>
        new($"{nameof(BranchAccessError)}.{nameof(BranchAccessError.MembershipNotFound)}", message);

    public static Error DatabaseError(string message) =>
        new($"{nameof(BranchAccessError)}.{nameof(BranchAccessError.DatabaseError)}", message);

    public static Error OperationCancelled(string message) =>
        new($"{nameof(BranchAccessError)}.{nameof(BranchAccessError.OperationCancelled)}", message);

    public static Error InternalServerError(string message) =>
        new($"{nameof(BranchAccessError)}.{nameof(BranchAccessError.InternalServerError)}", message);
}
