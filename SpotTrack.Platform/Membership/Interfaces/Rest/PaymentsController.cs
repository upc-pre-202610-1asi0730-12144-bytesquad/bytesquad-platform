using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest;

[ApiController]
[Route("api/v1/payments")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
[SwaggerTag("Payment initiation endpoints")]
public class PaymentsController(
    IPaymentCommandService paymentCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("membership")]
    [SwaggerOperation(
        Summary = "Initiate a membership payment for the authenticated admin",
        Description = "Creates a Stripe Checkout session for renewing the admin's own membership. Returns the checkout URL.",
        OperationId = "InitiateMembershipPayment")]
    [SwaggerResponse(StatusCodes.Status201Created, "Checkout session created", typeof(BusinessRegistrationResultResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Unrecognised membership plan")]
    [SwaggerResponse(StatusCodes.Status502BadGateway, "Stripe session creation failed")]
    public async Task<IActionResult> InitiateMembershipPayment(
        [FromBody] InitiateMembershipPaymentResource resource,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<EMembershipPlan>(resource.MembershipPlan, ignoreCase: true, out var plan))
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status400BadRequest, (Enum?)null,
                $"Unrecognised membership plan '{resource.MembershipPlan}'. Valid values: Basic, Mid, Premium.");

        var userId = ((User)HttpContext.Items["User"]!).Id;
        var (amount, currency) = plan.ToPrice();
        var command = new InitiateMembershipPaymentCommand(userId, plan, amount, currency);
        var result = await paymentCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status502BadGateway, result.Error, result.Message);

        return StatusCode(StatusCodes.Status201Created, new BusinessRegistrationResultResource(result.Value!));
    }
}
