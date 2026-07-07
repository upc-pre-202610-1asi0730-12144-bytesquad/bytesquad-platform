using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Monitoring.Application.CommandServices;

public interface IAnomalyCommandService
{
    Task<Result<Anomaly>> Handle(ReportAnomalyCommand command, CancellationToken cancellationToken);
}
