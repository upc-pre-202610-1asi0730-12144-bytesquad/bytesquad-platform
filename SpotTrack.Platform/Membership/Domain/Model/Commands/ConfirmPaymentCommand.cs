namespace SpotTrack.Platform.Memberships.Domain.Model.Commands;

public record ConfirmPaymentCommand(Guid PaymentId, string GatewaySessionId);
