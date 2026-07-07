using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;

namespace SpotTrack.Platform.Gyms.Application.Internal.QueryServices;

public class EquipmentQueryService(IEquipmentRepository equipmentRepository) : IEquipmentQueryService
{
    public async Task<Equipment?> Handle(GetEquipmentByIdQuery query, CancellationToken cancellationToken)
        => await equipmentRepository.FindByIdAsync(query.EquipmentId, cancellationToken);

    public async Task<IEnumerable<Equipment>> Handle(GetEquipmentByAdminIdQuery query, CancellationToken cancellationToken)
        => await equipmentRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);

    public async Task<IEnumerable<Equipment>> Handle(GetEquipmentByGymIdQuery query, CancellationToken cancellationToken)
        => await equipmentRepository.FindAllByGymIdAsync(query.GymId, cancellationToken);
}
