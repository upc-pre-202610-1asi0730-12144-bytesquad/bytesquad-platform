using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;

namespace SpotTrack.Platform.Monitoring.Application.QueryServices;

public interface ISensorQueryService
{
    Task<IEnumerable<Sensor>> Handle(GetAllSensorsByTypeQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Sensor>> Handle(GetSensorsByAdminIdAndTypeQuery query, CancellationToken cancellationToken);
}
