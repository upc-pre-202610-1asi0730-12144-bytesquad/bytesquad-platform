namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

public record MembershipResource(
    int Id,
    int ClientId,
    string Plan,
    decimal Amount,
    string Currency,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Status,
    string? PendingDowngradePlan);
