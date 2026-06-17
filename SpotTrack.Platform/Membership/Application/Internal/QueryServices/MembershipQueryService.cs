using SpotTrack.Platform.Memberships.Application.QueryServices;
using SpotTrack.Platform.Memberships.Domain.Repositories;

namespace SpotTrack.Platform.Memberships.Application.Internal.QueryServices;

public class MembershipQueryService(IMembershipRepository membershipRepository)
    : IMembershipQueryService
{
}
