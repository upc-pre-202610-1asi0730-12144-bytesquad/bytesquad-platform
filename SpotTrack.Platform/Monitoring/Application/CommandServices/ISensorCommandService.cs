using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Monitoring.Application.CommandServices;

public interface ISensorCommandService
{
    Task<Result<Sensor>> Handle(RegisterSensorCommand command, CancellationToken cancellationToken);
    Task<Result<Sensor>> Handle(MarkSensorDisconnectedCommand command, CancellationToken cancellationToken);
    Task<Result<Sensor>> Handle(MarkSensorReconnectedCommand command, CancellationToken cancellationToken);
}
