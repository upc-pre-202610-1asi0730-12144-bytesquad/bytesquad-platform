namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record CreateRenewMembershipCommand(int MembershipId, DateTimeOffset NewEndDate);
