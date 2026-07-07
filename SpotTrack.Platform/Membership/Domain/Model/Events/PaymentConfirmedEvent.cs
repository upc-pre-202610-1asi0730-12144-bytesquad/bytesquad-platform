using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.ValueObjects;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Memberships.Domain.Model.Events;

public record PaymentConfirmedEvent(
    Guid PaymentId,
    Guid? PendingRegistrationId,
    int? UserId,
    int? MembershipId,
    EMembershipPlan MembershipPlan,
    decimal Amount,
    string Currency,
    PaymentPurpose Purpose) : IEvent
{
    public static PaymentConfirmedEvent FromPayment(Payment payment) =>
        new(payment.PaymentId,
            payment.PendingRegistrationId,
            payment.UserId,
            payment.MembershipId,
            payment.MembershipPlan,
            payment.Amount,
            payment.Currency,
            payment.Purpose);
}
