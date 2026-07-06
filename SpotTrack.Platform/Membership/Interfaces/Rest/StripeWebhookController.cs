using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Infrastructure.Stripe;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest;

[ApiController]
[Route("api/v1/webhooks/stripe")]
[AllowAnonymous]
[SwaggerTag("Stripe webhook receiver")]
public class StripeWebhookController(
    IPaymentCommandService paymentCommandService,
    IOptions<StripeSettings> stripeOptions,
    ILogger<StripeWebhookController> logger) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Receive Stripe webhook events", OperationId = "StripeWebhook")]
    [SwaggerResponse(StatusCodes.Status200OK, "Event processed")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Stripe signature")]
    public async Task<IActionResult> HandleWebhook(CancellationToken cancellationToken)
    {
        var rawBody = await new StreamReader(Request.Body).ReadToEndAsync(cancellationToken);
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(rawBody, signature, stripeOptions.Value.WebhookSecret);
        }
        catch (StripeException ex)
        {
            logger.LogWarning("Stripe webhook signature validation failed: {Message}", ex.Message);
            return BadRequest();
        }

        if (!Guid.TryParse(stripeEvent.Data.Object is Stripe.Checkout.Session session
                ? session.Metadata.GetValueOrDefault("paymentId")
                : null,
            out var paymentId))
        {
            logger.LogWarning("Stripe webhook: missing or invalid paymentId in metadata for event {EventId}", stripeEvent.Id);
            return Ok();
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                var completedSession = (Stripe.Checkout.Session)stripeEvent.Data.Object;
                var confirmResult = await paymentCommandService.Handle(
                    new ConfirmPaymentCommand(paymentId, completedSession.Id), cancellationToken);
                if (confirmResult.IsFailure)
                    logger.LogError("Stripe webhook: failed to confirm payment {PaymentId}: {Message}",
                        paymentId, confirmResult.Message);
                break;

            case "checkout.session.expired":
                var failResult = await paymentCommandService.Handle(
                    new FailPaymentCommand(paymentId), cancellationToken);
                if (failResult.IsFailure)
                    logger.LogError("Stripe webhook: failed to mark payment {PaymentId} as failed: {Message}",
                        paymentId, failResult.Message);
                break;

            default:
                logger.LogDebug("Stripe webhook: unhandled event type {EventType}", stripeEvent.Type);
                break;
        }

        return Ok();
    }
}
