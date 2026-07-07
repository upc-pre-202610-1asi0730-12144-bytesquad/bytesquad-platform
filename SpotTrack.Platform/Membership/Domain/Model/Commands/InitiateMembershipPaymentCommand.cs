namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record InitiateMembershipPaymentCommand(int UserId, EMembershipPlan MembershipPlan, decimal Amount, string Currency);
