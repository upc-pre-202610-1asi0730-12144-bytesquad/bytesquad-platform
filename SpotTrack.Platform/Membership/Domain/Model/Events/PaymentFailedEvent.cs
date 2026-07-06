using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.ValueObjects;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record PaymentFailedEvent(
    Guid PaymentId,
    Guid? PendingRegistrationId,
    int? UserId,
    int? MembershipId,
    PaymentPurpose Purpose) : IEvent
{
    public static PaymentFailedEvent FromPayment(Payment payment) =>
        new(payment.PaymentId,
            payment.PendingRegistrationId,
            payment.UserId,
            payment.MembershipId,
            payment.Purpose);
}
