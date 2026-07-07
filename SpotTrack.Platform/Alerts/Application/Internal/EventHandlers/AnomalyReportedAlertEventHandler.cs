using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Alerts.Application.CommandServices;
using SpotTrack.Platform.Alerts.Domain.Model.Commands;
using SpotTrack.Platform.Alerts.Domain.Model.ValueObjects;
using SpotTrack.Platform.Monitoring.Domain.Model.Events;
using SpotTrack.Platform.Shared.Application.Internal.EventHandlers;

namespace SpotTrack.Platform.Alerts.Application.Internal.EventHandlers;

public class AnomalyReportedAlertEventHandler(
    IAlertCommandService alertCommandService,
    ILogger<AnomalyReportedAlertEventHandler> logger)
    : IEventHandler<AnomalyReportedEvent>
{
    public async Task Handle(AnomalyReportedEvent notification, CancellationToken cancellationToken)
    {
        var command = new CreateAlertCommand(
            notification.AdminId,
            null,
            EAlertSeverity.Warning,
            $"Anomalía detectada en sensor #{notification.SensorId}: {notification.AnomalyType} — {notification.Description}");

        var result = await alertCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            logger.LogError("AnomalyReported: failed to create alert for admin {AdminId}: {Message}", notification.AdminId, result.Message);
    }
}
