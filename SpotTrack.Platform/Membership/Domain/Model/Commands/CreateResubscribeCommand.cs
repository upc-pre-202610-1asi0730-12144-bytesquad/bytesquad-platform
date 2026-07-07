namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record CreateResubscribeCommand(int MembershipId, DateTimeOffset NewStartDate, DateTimeOffset NewEndDate);
