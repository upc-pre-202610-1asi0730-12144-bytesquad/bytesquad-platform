namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

public record GrantBranchAccessResource(int MembershipId, int BranchId, int GrantedByAdminId);
