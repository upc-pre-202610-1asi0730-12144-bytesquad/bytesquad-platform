namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record CreateActivateMembershipCommand(
    int ClientId,
    EMembershipPlan Plan,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);
