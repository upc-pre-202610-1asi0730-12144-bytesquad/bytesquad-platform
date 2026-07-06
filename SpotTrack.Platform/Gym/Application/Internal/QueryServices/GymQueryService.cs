using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;

namespace SpotTrack.Platform.Gyms.Application.Internal.QueryServices;

public class GymQueryService(
    IGymRepository gymRepository,
    IEquipmentRepository equipmentRepository)
    : IGymQueryService
{
    public async Task<IReadOnlyCollection<Gym>> Handle(
        GetAllGymsQuery query, CancellationToken cancellationToken)
    {
        var gyms = await gymRepository.ListAsync(cancellationToken);
        return gyms.ToList();
    }

    public async Task<IReadOnlyCollection<Branch>?> Handle(
        GetBranchesByGymIdQuery query, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.FindByIdWithBranchesAsync(query.GymId, cancellationToken);
        return gym?.Branches;
    }

    public async Task<IReadOnlyCollection<Zone>?> Handle(
        GetZonesByGymIdQuery query, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.FindByIdWithBranchesAndZonesAsync(query.GymId, cancellationToken);
        return gym?.Branches.SelectMany(b => b.Zones).ToList();
    }

    public async Task<IReadOnlyCollection<Equipment>?> Handle(
        GetEquipmentsByGymIdQuery query, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.FindByIdWithBranchesAndZonesAsync(query.GymId, cancellationToken);
        if (gym is null)
            return null;

        var zoneIds = gym.Branches.SelectMany(b => b.Zones).Select(z => z.Id).ToList();
        var equipment = await equipmentRepository.FindAllByZoneIdsAsync(zoneIds, cancellationToken);
        return equipment.ToList();
    }
}
