namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record InitiateBusinessPaymentCommand(
    Guid PendingRegistrationId,
    EMembershipPlan MembershipPlan,
    decimal Amount,
    string Currency);
