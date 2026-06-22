using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;

public static class BranchAccessResourceFromEntityAssembler
{
    public static BranchAccessResource ToResourceFromEntity(BranchAccess branchAccess) =>
        new(branchAccess.Id,
            branchAccess.MembershipId,
            branchAccess.BranchId,
            branchAccess.Status.ToString(),
            branchAccess.GrantedByAdminId);
}
