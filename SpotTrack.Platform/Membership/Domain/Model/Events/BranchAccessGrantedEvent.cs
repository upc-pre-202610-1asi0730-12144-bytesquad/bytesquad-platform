using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record BranchAccessGrantedEvent(
    int BranchAccessId,
    int MembershipId,
    int BranchId,
    int GrantedByAdminId) : IEvent
{
    public static BranchAccessGrantedEvent FromBranchAccess(BranchAccess branchAccess) =>
        new(branchAccess.Id,
            branchAccess.MembershipId,
            branchAccess.BranchId,
            branchAccess.GrantedByAdminId);
}
