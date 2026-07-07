using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;

namespace SpotTrack.Platform.Gyms.Application.Internal.QueryServices;

public class GymQueryService(IGymRepository gymRepository) : IGymQueryService
{
    public async Task<Gym?> Handle(GetGymByIdQuery query, CancellationToken cancellationToken)
        => await gymRepository.FindByIdAsync(query.GymId, cancellationToken);

    public async Task<Gym?> Handle(GetGymByAdminIdQuery query, CancellationToken cancellationToken)
        => await gymRepository.FindByAdminIdAsync(query.AdminId, cancellationToken);

    public async Task<IEnumerable<Gym>> Handle(GetAllGymsQuery query, CancellationToken cancellationToken)
        => await gymRepository.ListAsync(cancellationToken);
}
