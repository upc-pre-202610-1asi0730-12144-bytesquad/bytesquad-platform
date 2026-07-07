using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record MembershipCancellationUndoneEvent(int MembershipId, int ClientId) : IEvent
{
    public static MembershipCancellationUndoneEvent FromMembership(Membership membership) =>
        new(membership.Id, membership.ClientId);
}
