using Cortex.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Events;
using SpotTrack.Platform.Memberships.Domain.Repositories;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Memberships.Infrastructure.BackgroundServices;

public class MembershipExpirationBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<MembershipExpirationBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessExpirationBatchAsync(stoppingToken);
            await Task.Delay(Interval, stoppingToken);
        }
    }

    private async Task ProcessExpirationBatchAsync(CancellationToken stoppingToken)
    {
        var now = DateTimeOffset.UtcNow;
        IReadOnlyList<int> expiredIds;

        await using (var queryScope = scopeFactory.CreateAsyncScope())
        {
            var repo = queryScope.ServiceProvider.GetRequiredService<IMembershipRepository>();
            var expired = await repo.FindAllExpiredAsync(now, stoppingToken);
            expiredIds = expired.Select(m => m.Id).ToList();
        }

        if (expiredIds.Count == 0) return;

        logger.LogInformation(
            "Membership expiration batch started: {Count} membership(s) to process.", expiredIds.Count);

        foreach (var id in expiredIds)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var repo = scope.ServiceProvider.GetRequiredService<IMembershipRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                var membership = await repo.FindByIdAsync(id, stoppingToken);
                if (membership is null)
                {
                    logger.LogWarning("Membership {MembershipId} not found during expiration batch.", id);
                    continue;
                }

                if (membership.Status == EMembershipStatus.Active)
                {
                    membership.Expire();
                    repo.Update(membership);
                    await unitOfWork.CompleteAsync(stoppingToken);
                    await mediator.PublishAsync(
                        MembershipExpiredEvent.FromMembership(membership), stoppingToken);
                    logger.LogInformation("Membership {MembershipId} expired.", id);
                }
                else if (membership.Status == EMembershipStatus.PendingCancellation)
                {
                    membership.CompleteCancellation();
                    repo.Update(membership);
                    await unitOfWork.CompleteAsync(stoppingToken);
                    await mediator.PublishAsync(
                        MembershipCancelledEvent.FromMembership(membership), stoppingToken);
                    logger.LogInformation("Membership {MembershipId} cancellation completed.", id);
                }
                // Status changed between Phase 1 and Phase 2 → already handled elsewhere, skip silently
            }
            catch (InvalidOperationException ex)
            {
                logger.LogWarning(ex,
                    "Membership {MembershipId} status changed between batch query and processing — skipping.", id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to process membership {MembershipId} during expiration batch.", id);
            }
        }
    }
}
