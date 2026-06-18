using SpotTrack.Platform.Memberships.Application.QueryServices;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.Queries;
using SpotTrack.Platform.Memberships.Domain.Repositories;

namespace SpotTrack.Platform.Memberships.Application.Internal.QueryServices;

public class MembershipQueryService(IMembershipRepository membershipRepository)
    : IMembershipQueryService
{
    public async Task<IEnumerable<Membership>> Handle(
        GetAllMembershipsByClientIdQuery query,
        CancellationToken cancellationToken)
        => await membershipRepository.FindAllByClientIdAsync(query.ClientId, cancellationToken);
    

    public async Task<Membership?> Handle(GetMembershipByIdQuery query, CancellationToken cancellationToken)
        => await membershipRepository.FindByIdAsync(query.MembershipId, cancellationToken);
}


