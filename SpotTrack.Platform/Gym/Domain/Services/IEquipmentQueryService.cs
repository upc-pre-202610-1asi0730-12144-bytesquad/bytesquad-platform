using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;

namespace SpotTrack.Platform.Gyms.Domain.Services;

public interface IEquipmentQueryService
{
    Task<IEnumerable<Equipment>> Handle(GetEquipmentByAdminIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Equipment>> Handle(GetEquipmentByGymIdQuery query, CancellationToken cancellationToken);
}
