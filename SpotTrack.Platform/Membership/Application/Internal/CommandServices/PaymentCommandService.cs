using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Domain.Model.Events;
using SpotTrack.Platform.Memberships.Domain.Repositories;
using SpotTrack.Platform.Memberships.Infrastructure.Stripe;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Memberships.Application.Internal.CommandServices;

public class PaymentCommandService(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IOptions<StripeSettings> stripeOptions,
    ILogger<PaymentCommandService> logger)
    : IPaymentCommandService
{
    private StripeSettings Stripe => stripeOptions.Value;

    public async Task<Result<string>> Handle(InitiateBusinessPaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = Payment.ForBusinessRegistration(command);

        try
        {
            await paymentRepository.AddAsync(payment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return Result<string>.Failure(MembershipError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error persisting payment for pending registration {PendingRegistrationId}", command.PendingRegistrationId);
            return Result<string>.Failure(MembershipError.DatabaseError, "A database error occurred.");
        }

        var sessionService = new SessionService();
        var options = new SessionCreateOptions
        {
            Mode = "payment",
            LineItems =
            [
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = command.Currency,
                        UnitAmount = command.MembershipPlan.ToStripeAmount(),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"SpotTrack {command.MembershipPlan} Membership"
                        }
                    },
                    Quantity = 1
                }
            ],
            SuccessUrl = $"{Stripe.SuccessUrl}?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = Stripe.CancelUrl,
            Metadata = new Dictionary<string, string>
            {
                ["paymentId"] = payment.PaymentId.ToString(),
                ["pendingRegistrationId"] = command.PendingRegistrationId.ToString()
            }
        };

        try
        {
            var session = await sessionService.CreateAsync(options, cancellationToken: cancellationToken);
            return Result<string>.Success(session.Url);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Stripe session creation failed for payment {PaymentId}", payment.PaymentId);
            return Result<string>.Failure(MembershipError.StripeError, "Failed to create Stripe checkout session.");
        }
    }

    public async Task<Result> Handle(ConfirmPaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.FindByPaymentIdAsync(command.PaymentId, cancellationToken);
        if (payment is null)
            return Result.Failure(MembershipError.PaymentNotFound, $"Payment {command.PaymentId} not found.");

        try
        {
            payment.Confirm(command.GatewaySessionId);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(MembershipError.InvalidPaymentState, ex.Message);
        }

        try
        {
            paymentRepository.Update(payment);
            await unitOfWork.CompleteAsync(cancellationToken);
            await mediator.PublishAsync(PaymentConfirmedEvent.FromPayment(payment), cancellationToken);
            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            return Result.Failure(MembershipError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error confirming payment {PaymentId}", command.PaymentId);
            return Result.Failure(MembershipError.DatabaseError, "A database error occurred.");
        }
    }

    public async Task<Result> Handle(FailPaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.FindByPaymentIdAsync(command.PaymentId, cancellationToken);
        if (payment is null)
            return Result.Failure(MembershipError.PaymentNotFound, $"Payment {command.PaymentId} not found.");

        try
        {
            payment.Fail();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(MembershipError.InvalidPaymentState, ex.Message);
        }

        try
        {
            paymentRepository.Update(payment);
            await unitOfWork.CompleteAsync(cancellationToken);
            await mediator.PublishAsync(PaymentFailedEvent.FromPayment(payment), cancellationToken);
            return Result.Success();
        }
        catch (OperationCanceledException)
        {
            return Result.Failure(MembershipError.OperationCancelled, "The operation was cancelled.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error failing payment {PaymentId}", command.PaymentId);
            return Result.Failure(MembershipError.DatabaseError, "A database error occurred.");
        }
    }
}
