using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;

namespace SpotTrack.Platform.Gyms.Application.Internal.QueryServices;

public class GymQueryService(IGymRepository gymRepository) : IGymQueryService
{
    public async Task<Gym?> Handle(GetGymByAdminIdQuery query, CancellationToken cancellationToken)
        => await gymRepository.FindByAdminIdAsync(query.AdminId, cancellationToken);
}
