using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;

public static class GrantBranchAccessCommandFromResourceAssembler
{
    public static CreateGrantBranchAccessCommand ToCommandFromResource(GrantBranchAccessResource resource) =>
        new(resource.MembershipId, resource.BranchId, resource.GrantedByAdminId);
}
