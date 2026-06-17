using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record BranchAccessDeniedEvent(
    int BranchAccessId,
    int MembershipId,
    int BranchId,
    int GrantedByAdminId) : IEvent
{
    public static BranchAccessDeniedEvent FromBranchAccess(BranchAccess branchAccess) =>
        new(branchAccess.Id,
            branchAccess.MembershipId,
            branchAccess.BranchId,
            branchAccess.GrantedByAdminId);
}
