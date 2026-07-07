using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;

namespace SpotTrack.Platform.Gyms.Domain.Services;

public interface IGymQueryService
{
    /// <returns>null if the gym does not exist, otherwise its branches (possibly empty).</returns>
    Task<IReadOnlyCollection<Branch>?> Handle(GetBranchesByGymIdQuery query, CancellationToken cancellationToken);

    /// <returns>null if the gym does not exist, otherwise its zones across all branches (possibly empty).</returns>
    Task<IReadOnlyCollection<Zone>?> Handle(GetZonesByGymIdQuery query, CancellationToken cancellationToken);

    /// <returns>null if the gym does not exist, otherwise its equipment across all zones (possibly empty).</returns>
    Task<IReadOnlyCollection<Equipment>?> Handle(GetEquipmentsByGymIdQuery query, CancellationToken cancellationToken);

    Task<Gym?> Handle(GetGymByIdQuery query, CancellationToken cancellationToken);
    Task<Gym?> Handle(GetGymByAdminIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Gym>> Handle(GetAllGymsQuery query, CancellationToken cancellationToken);
    Task<Gym?> Handle(GetGymWithBranchesByIdQuery query, CancellationToken cancellationToken);
}
