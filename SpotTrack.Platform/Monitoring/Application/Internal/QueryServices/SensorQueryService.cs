using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;
using SpotTrack.Platform.Monitoring.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Application.Internal.QueryServices;

public class SensorQueryService(ISensorRepository sensorRepository) : ISensorQueryService
{
    public async Task<IEnumerable<Sensor>> Handle(GetAllSensorsByTypeQuery query, CancellationToken cancellationToken)
        => await sensorRepository.FindAllBySensorTypeAsync(query.SensorType, cancellationToken);

    public async Task<IEnumerable<Sensor>> Handle(GetSensorsByAdminIdAndTypeQuery query, CancellationToken cancellationToken)
        => await sensorRepository.FindAllByAdminIdAndTypeAsync(query.AdminId, query.SensorType, cancellationToken);
}
