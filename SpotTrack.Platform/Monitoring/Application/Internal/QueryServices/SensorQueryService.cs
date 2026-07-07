using SpotTrack.Platform.Gyms.Interfaces.Acl;
using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;
using SpotTrack.Platform.Monitoring.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.QueryServices;

public class SensorQueryService(
    ISensorRepository sensorRepository,
    IGymContextFacade gymContextFacade)
    : ISensorQueryService
{
    public async Task<IEnumerable<Sensor>> Handle(GetSensorsByAdminIdQuery query, CancellationToken cancellationToken)
    {
        var equipmentIds = await gymContextFacade.GetEquipmentIdsByAdminIdAsync(query.AdminId, cancellationToken);
        return await sensorRepository.FindAllByEquipmentIdsAsync(equipmentIds, cancellationToken);
    }
}
