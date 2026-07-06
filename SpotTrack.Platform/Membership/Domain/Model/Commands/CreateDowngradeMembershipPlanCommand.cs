namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record CreateDowngradeMembershipPlanCommand(int MembershipId, EMembershipPlan NewPlan);
