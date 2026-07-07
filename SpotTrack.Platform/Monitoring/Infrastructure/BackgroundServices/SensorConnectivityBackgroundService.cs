using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Repositories;

namespace SpotTrack.Platform.Monitoring.Infrastructure.BackgroundServices;

/// <summary>
///     Simulates Edge IoT connectivity flakiness — there's no real hardware to report a lost
///     connection, so this randomly drops an online sensor's link, and restores it on its own
///     after a short while (per US21's "Escenario 2: Reconexión exitosa").
/// </summary>
public class SensorConnectivityBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<SensorConnectivityBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan ReconnectAfter = TimeSpan.FromMinutes(1);
    private const double DisconnectProbability = 0.3;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await SimulateConnectivityAsync(stoppingToken);
            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task SimulateConnectivityAsync(CancellationToken stoppingToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var sensorRepository = scope.ServiceProvider.GetRequiredService<ISensorRepository>();
        var commandService = scope.ServiceProvider.GetRequiredService<ISensorCommandService>();

        var online = await sensorRepository.FindAllOnlineAsync(stoppingToken);
        foreach (var sensor in online)
        {
            if (Random.Shared.NextDouble() < DisconnectProbability)
            {
                var result = await commandService.Handle(new MarkSensorDisconnectedCommand(sensor.Id), stoppingToken);
                if (!result.IsFailure)
                    logger.LogInformation("Sensor {SensorId} simulated disconnection.", sensor.Id);
            }
        }

        var threshold = DateTimeOffset.UtcNow - ReconnectAfter;
        var offlineLongEnough = await sensorRepository.FindAllOfflineSinceAsync(threshold, stoppingToken);
        foreach (var sensor in offlineLongEnough)
        {
            var result = await commandService.Handle(new MarkSensorReconnectedCommand(sensor.Id), stoppingToken);
            if (!result.IsFailure)
                logger.LogInformation("Sensor {SensorId} auto-reconnected.", sensor.Id);
        }
    }
}
