using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;

public static class UpgradeMembershipPlanCommandFromResourceAssembler
{
    public static CreateUpgradeMembershipPlanCommand ToCommandFromResource(
        int membershipId,
        UpgradeMembershipPlanResource resource) =>
        new(membershipId, Enum.Parse<EMembershipPlan>(resource.NewPlan, ignoreCase: true));
}
