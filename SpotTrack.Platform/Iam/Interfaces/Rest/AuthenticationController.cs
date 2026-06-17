using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Application.CommandServices;
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
        Description = "Creates a new user account with the given username, password and role.",
        OperationId = "SignUp")]
    [SwaggerResponse(StatusCodes.Status201Created, "User created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid role provided")]
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
