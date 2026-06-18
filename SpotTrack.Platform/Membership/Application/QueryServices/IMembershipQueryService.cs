using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.Queries;

namespace SpotTrack.Platform.Memberships.Application.QueryServices;

public interface IMembershipQueryService
{
    Task<IEnumerable<Membership>> Handle(GetAllMembershipsByClientIdQuery query, CancellationToken cancellationToken);
}
