using SpotTrack.Platform.Gyms.Interfaces.Acl;
using SpotTrack.Platform.Memberships.Application.QueryServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Queries;

namespace SpotTrack.Platform.Gyms.Application.Acl;

public class MembershipContextFacade(IMembershipQueryService membershipQueryService) : IMembershipContextFacade
{
    private static int PlanToBranchLimit(EMembershipPlan plan) => plan switch
    {
        EMembershipPlan.Basic => 1,
        EMembershipPlan.Standard => 3,
        EMembershipPlan.Premium => int.MaxValue,
        _ => 0
    };

    public async Task<int> GetBranchLimitForAdminAsync(int adminId, CancellationToken cancellationToken)
    {
        var memberships = await membershipQueryService.Handle(
            new GetAllMembershipsByClientIdQuery(adminId), cancellationToken);

        var activeMembership = memberships
            .Where(m => m.Status == EMembershipStatus.Active)
            .OrderByDescending(m => m.Plan)
            .FirstOrDefault();

        return activeMembership is null ? 0 : PlanToBranchLimit(activeMembership.Plan);
    }
}
