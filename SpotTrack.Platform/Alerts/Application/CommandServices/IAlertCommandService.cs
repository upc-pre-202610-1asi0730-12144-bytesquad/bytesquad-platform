using SpotTrack.Platform.Alerts.Domain.Model.Aggregates;
using SpotTrack.Platform.Alerts.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Alerts.Application.CommandServices;

public interface IAlertCommandService
{
    Task<Result<Alert>> Handle(CreateAlertCommand command, CancellationToken cancellationToken);
    Task<Result<Alert>> Handle(ResolveAlertCommand command, CancellationToken cancellationToken);
}
