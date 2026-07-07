using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Memberships.Domain.Model.Aggregates;

public class Payment
{
    private Payment() { }

    private Payment(
        Guid paymentId,
        int? userId,
        Guid? pendingRegistrationId,
        int? membershipId,
        EMembershipPlan membershipPlan,
        decimal amount,
        string currency,
        PaymentPurpose purpose)
    {
        PaymentId = paymentId;
        UserId = userId;
        PendingRegistrationId = pendingRegistrationId;
        MembershipId = membershipId;
        MembershipPlan = membershipPlan;
        Amount = amount;
        Currency = currency;
        Purpose = purpose;
        Status = PaymentStatus.Pending;
    }

    public static Payment ForBusinessRegistration(InitiateBusinessPaymentCommand command) =>
        new(Guid.NewGuid(), null, command.PendingRegistrationId, null,
            command.MembershipPlan, command.Amount, command.Currency,
            PaymentPurpose.BusinessRegistration);

    public static Payment ForMembershipRenewal(InitiateMembershipPaymentCommand command) =>
        new(Guid.NewGuid(), command.UserId, null, null,
            command.MembershipPlan, command.Amount, command.Currency,
            PaymentPurpose.NewMembership);

    public Guid PaymentId { get; private set; }
    public int? UserId { get; private set; }
    public Guid? PendingRegistrationId { get; private set; }
    public int? MembershipId { get; private set; }
    public EMembershipPlan MembershipPlan { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }
    public string? GatewayTransactionId { get; private set; }
    public PaymentPurpose Purpose { get; private set; }

    public void Confirm(string gatewaySessionId)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm a payment with status {Status}.");

        Status = PaymentStatus.Confirmed;
        GatewayTransactionId = gatewaySessionId;
    }

    public void Fail()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException($"Cannot fail a payment with status {Status}.");

        Status = PaymentStatus.Failed;
    }
}
