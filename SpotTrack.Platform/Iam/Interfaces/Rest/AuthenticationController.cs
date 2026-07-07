using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Application.CommandServices;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Iam.Interfaces.Rest.Resources;
using SpotTrack.Platform.Iam.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/authentication")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
[SwaggerTag("Authentication endpoints")]
public class AuthenticationController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("sign-up")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign up a new user",
        Description = "Creates a new client user account with the given username and password.",
        OperationId = "SignUp")]
    [SwaggerResponse(StatusCodes.Status201Created, "User created successfully")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Username already taken")]
    public async Task<IActionResult> SignUp(
        [FromBody] SignUpResource resource,
        CancellationToken cancellationToken)
    {
        var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return IamActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return IamActionResultAssembler.ToSignUpSuccessActionResult(this);
    }

    [HttpPatch("me/password")]
    [SwaggerOperation(
        Summary = "Change the authenticated user's password",
        Description = "Verifies the current password and replaces it with the new one. Returns 400 if the current password is wrong.",
        OperationId = "ChangePassword")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Password changed successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Current password is incorrect")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordResource resource,
        CancellationToken cancellationToken)
    {
        var userId = ((User)HttpContext.Items["User"]!).Id;
        var command = ChangePasswordCommandFromResourceAssembler.ToCommandFromResource(userId, resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return IamActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return NoContent();
    }

    [HttpPost("sign-in")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Sign in a user",
        Description = "Authenticates a user and returns a JWT token.",
        OperationId = "SignIn")]
    [SwaggerResponse(StatusCodes.Status200OK, "Authentication successful", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials")]
    public async Task<IActionResult> SignIn(
        [FromBody] SignInResource resource,
        CancellationToken cancellationToken)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return IamActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        var (user, token) = result.Value!;
        return IamActionResultAssembler.ToSignInSuccessActionResult(user, token, this);
    }
}
