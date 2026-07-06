using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Memberships.Domain.Model.Events;
using SpotTrack.Platform.Shared.Application.Internal.EventHandlers;

namespace SpotTrack.Platform.Memberships.Application.Internal.EventHandlers;

public class PaymentFailedEventHandler(ILogger<PaymentFailedEventHandler> logger)
    : IEventHandler<PaymentFailedEvent>
{
    public Task Handle(PaymentFailedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogWarning(
            "Payment {PaymentId} failed for pending registration {RegistrationId}",
            notification.PaymentId,
            notification.PendingRegistrationId);
        return Task.CompletedTask;
    }
}
