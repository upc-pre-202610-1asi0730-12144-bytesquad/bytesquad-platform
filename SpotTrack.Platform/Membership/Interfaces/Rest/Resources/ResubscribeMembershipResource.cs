namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

public record ResubscribeMembershipResource(DateTimeOffset NewStartDate, DateTimeOffset NewEndDate);
