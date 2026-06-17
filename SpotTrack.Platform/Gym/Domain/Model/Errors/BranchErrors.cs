using SpotTrack.Platform.Shared.Domain.Model;

namespace SpotTrack.Platform.Gyms.Domain.Model.Errors;

public static class BranchErrors
{
    public static Error BranchNotFound(string message) =>
        new($"{nameof(BranchError)}.{nameof(BranchError.BranchNotFound)}", message);
}
