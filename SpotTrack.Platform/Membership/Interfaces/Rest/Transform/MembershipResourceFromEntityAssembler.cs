using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;

public static class MembershipResourceFromEntityAssembler
{
    public static MembershipResource ToResourceFromEntity(Membership membership) =>
        new(membership.Id,
            membership.ClientId,
            membership.Plan.ToString(),
            membership.Plan.ToPrice().Amount,
            membership.Plan.ToPrice().Currency,
            membership.StartDate,
            membership.EndDate,
            membership.Status.ToString(),
            membership.PendingDowngradePlan?.ToString());
}
