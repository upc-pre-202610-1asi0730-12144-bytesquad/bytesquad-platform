using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Reservations.Application.CommandServices;
using SpotTrack.Platform.Reservations.Domain.Model.Commands;
using SpotTrack.Platform.Reservations.Domain.Repositories;

namespace SpotTrack.Platform.Reservations.Infrastructure.BackgroundServices;

/// <summary>
///     Backstop for reservations whose timer expired without the client ending them
///     manually — ends them the same way CreateEndReservationCommand does.
/// </summary>
public class ReservationTimerExpiryBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<ReservationTimerExpiryBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessExpiredReservationsAsync(stoppingToken);
            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task ProcessExpiredReservationsAsync(CancellationToken stoppingToken)
    {
        var now = DateTimeOffset.UtcNow;
        IReadOnlyList<int> expiredIds;

        await using (var queryScope = scopeFactory.CreateAsyncScope())
        {
            var repo = queryScope.ServiceProvider.GetRequiredService<IReservationRepository>();
            var expired = await repo.FindAllExpiredAsync(now, stoppingToken);
            expiredIds = expired.Select(r => r.Id).ToList();
        }

        if (expiredIds.Count == 0) return;

        logger.LogInformation(
            "Reservation timer expiry batch started: {Count} reservation(s) to process.", expiredIds.Count);

        foreach (var id in expiredIds)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var commandService = scope.ServiceProvider.GetRequiredService<IReservationCommandService>();

            try
            {
                var result = await commandService.Handle(new CreateEndReservationCommand(id), stoppingToken);
                if (result.IsFailure)
                    logger.LogWarning(
                        "Reservation {ReservationId} could not be auto-ended on timer expiry: {Error}",
                        id, result.Message);
                else
                    logger.LogInformation("Reservation {ReservationId} auto-ended on timer expiry.", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to process reservation {ReservationId} during timer expiry batch.", id);
            }
        }
    }
}
