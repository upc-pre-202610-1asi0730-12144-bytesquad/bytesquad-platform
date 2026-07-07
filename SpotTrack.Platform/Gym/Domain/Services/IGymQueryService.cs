using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;

namespace SpotTrack.Platform.Gyms.Domain.Services;

public interface IGymQueryService
{
    Task<Gym?> Handle(GetGymByAdminIdQuery query, CancellationToken cancellationToken);
}
