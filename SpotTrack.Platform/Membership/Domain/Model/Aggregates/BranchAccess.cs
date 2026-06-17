namespace SpotTrack.Platform.Memberships.Domain.Model.Aggregates;

public partial class BranchAccess
{
    private BranchAccess() { }

    private BranchAccess(int membershipId, int branchId, int grantedByAdminId, EBranchAccessStatus status)
    {
        MembershipId = membershipId;
        BranchId = branchId;
        GrantedByAdminId = grantedByAdminId;
        Status = status;
    }

    public int Id { get; private set; }
    public int MembershipId { get; private set; }
    public int BranchId { get; private set; }
    public EBranchAccessStatus Status { get; private set; }
    public int GrantedByAdminId { get; private set; }

    public static BranchAccess Grant(int membershipId, int branchId, int grantedByAdminId) =>
        new(membershipId, branchId, grantedByAdminId, EBranchAccessStatus.Granted);

    public static BranchAccess Deny(int membershipId, int branchId, int grantedByAdminId) =>
        new(membershipId, branchId, grantedByAdminId, EBranchAccessStatus.Denied);
}
