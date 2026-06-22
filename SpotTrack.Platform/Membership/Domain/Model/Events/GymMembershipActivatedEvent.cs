using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record GymMembershipActivatedEvent(
    int MembershipId,
    int ClientId,
    string Plan,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate) : IEvent
{
    public static GymMembershipActivatedEvent FromMembership(Membership membership) =>
        new(membership.Id,
            membership.ClientId,
            membership.Plan.ToString(),
            membership.StartDate,
            membership.EndDate);
}
