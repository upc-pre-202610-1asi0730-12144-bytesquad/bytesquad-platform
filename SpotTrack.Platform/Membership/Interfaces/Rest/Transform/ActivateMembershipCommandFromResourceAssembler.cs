using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;

public static class ActivateMembershipCommandFromResourceAssembler
{
    public static CreateActivateMembershipCommand ToCommandFromResource(ActivateMembershipResource resource) =>
        new(resource.ClientId,
            Enum.Parse<EMembershipPlan>(resource.Plan, ignoreCase: true),
            resource.StartDate,
            resource.EndDate);
}
