namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

public record MembershipResource(
    int Id,
    int ClientId,
    string Plan,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Status);
