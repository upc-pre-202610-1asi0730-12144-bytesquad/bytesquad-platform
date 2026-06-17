using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record GymMembershipRenewedEvent(
    int MembershipId,
    int ClientId,
    DateTimeOffset NewEndDate) : IEvent
{
    public static GymMembershipRenewedEvent FromMembership(Membership membership) =>
        new(membership.Id,
            membership.ClientId,
            membership.EndDate);
}
