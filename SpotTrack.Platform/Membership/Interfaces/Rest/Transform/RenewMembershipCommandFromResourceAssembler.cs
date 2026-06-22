using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;

public static class RenewMembershipCommandFromResourceAssembler
{
    public static CreateRenewMembershipCommand ToCommandFromResource(
        int membershipId,
        RenewMembershipResource resource) =>
        new(membershipId, resource.NewEndDate);
}
