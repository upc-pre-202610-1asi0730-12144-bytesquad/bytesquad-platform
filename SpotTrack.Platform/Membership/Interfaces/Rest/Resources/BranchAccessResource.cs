namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

public record BranchAccessResource(int Id, int MembershipId, int BranchId, string Status, int GrantedByAdminId);
