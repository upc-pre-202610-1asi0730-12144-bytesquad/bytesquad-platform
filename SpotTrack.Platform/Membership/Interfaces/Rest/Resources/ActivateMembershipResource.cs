namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

public record ActivateMembershipResource(
    int ClientId,
    string Plan,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);
