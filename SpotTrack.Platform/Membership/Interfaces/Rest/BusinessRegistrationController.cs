using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Iam.Interfaces.Acl;
using SpotTrack.Platform.Memberships.Application.CommandServices;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest;

[ApiController]
[Route("api/v1/register-business")]
[Produces(MediaTypeNames.Application.Json)]
[AllowAnonymous]
[SwaggerTag("Business registration endpoints")]
public class BusinessRegistrationController(
    IIamContextFacade iamFacade,
    IPaymentCommandService paymentCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Initiate business registration",
        Description = "Saves a pending registration and returns a Stripe checkout URL for membership payment.",
        OperationId = "RegisterBusiness")]
    [SwaggerResponse(StatusCodes.Status201Created, "Registration initiated", typeof(BusinessRegistrationResultResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid registration data or unrecognised membership tier")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Email already registered")]
    public async Task<IActionResult> RegisterBusiness(
        [FromBody] BusinessRegistrationResource resource,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<EMembershipPlan>(resource.MembershipTier, ignoreCase: true, out var plan))
            return BadRequest(problemDetailsFactory.CreateProblemDetails(
                HttpContext, StatusCodes.Status400BadRequest,
                $"Unrecognised membership tier '{resource.MembershipTier}'. Valid values: Basic, Mid, Premium."));

        var saveCommand = new SavePendingRegistrationCommand(
            resource.Email, resource.Password,
            resource.FirstName, resource.LastName, resource.PhoneNumber, resource.Dni,
            resource.CompanyName, resource.Ruc, resource.LegalStructure,
            resource.CompanyPhone, resource.CompanyEmail,
            resource.StreetAddress, resource.City, resource.District,
            resource.MembershipTier);

        var saveResult = await iamFacade.SavePendingRegistrationAsync(saveCommand, cancellationToken);
        if (saveResult.IsFailure)
            return StatusCode(StatusCodes.Status409Conflict,
                problemDetailsFactory.CreateProblemDetails(
                    HttpContext, StatusCodes.Status409Conflict, saveResult.Message));

        var (amount, currency) = plan.ToPrice();
        var paymentCommand = new InitiateBusinessPaymentCommand(saveResult.Value, plan, amount, currency);
        var paymentResult = await paymentCommandService.Handle(paymentCommand, cancellationToken);

        if (paymentResult.IsFailure)
            return StatusCode(StatusCodes.Status502BadGateway,
                problemDetailsFactory.CreateProblemDetails(
                    HttpContext, StatusCodes.Status502BadGateway, paymentResult.Message));

        return StatusCode(StatusCodes.Status201Created, new BusinessRegistrationResultResource(paymentResult.Value!));
    }
}
