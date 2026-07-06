using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record MembershipDowngradeRequestedEvent(
    int MembershipId,
    int ClientId,
    string NewPlan) : IEvent
{
    public static MembershipDowngradeRequestedEvent FromMembership(Membership membership) =>
        new(membership.Id,
            membership.ClientId,
            membership.PendingDowngradePlan!.Value.ToString());
}
