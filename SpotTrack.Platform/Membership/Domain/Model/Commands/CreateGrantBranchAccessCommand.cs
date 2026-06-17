namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record CreateGrantBranchAccessCommand(int MembershipId, int BranchId, int GrantedByAdminId);
