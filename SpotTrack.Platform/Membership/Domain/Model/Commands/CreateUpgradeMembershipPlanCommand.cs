namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record CreateUpgradeMembershipPlanCommand(int MembershipId, EMembershipPlan NewPlan);
