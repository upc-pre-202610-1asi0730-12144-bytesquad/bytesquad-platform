namespace SpotTrack.Platform.Memberships.Infrastructure.Stripe;

public record StripeSettings
{
    public string ApiKey { get; init; } = string.Empty;
    public string WebhookSecret { get; init; } = string.Empty;
    public string SuccessUrl { get; init; } = "http://localhost:4200/payment/success";
    public string CancelUrl { get; init; } = "http://localhost:4200/payment/cancel";
}
