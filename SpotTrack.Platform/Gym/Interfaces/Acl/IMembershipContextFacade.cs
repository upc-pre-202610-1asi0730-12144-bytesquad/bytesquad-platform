namespace SpotTrack.Platform.Gyms.Interfaces.Acl;

public interface IMembershipContextFacade
{
    Task<int> GetBranchLimitForAdminAsync(int adminId, CancellationToken cancellationToken);
}
