using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SpotTrack.Platform.Iam.Interfaces.Rest.Transform;

public static class IamActionResultAssembler
{
    private static int MapErrorToStatusCode(Enum error) => error switch
    {
        IamError.InvalidCredentials => StatusCodes.Status401Unauthorized,
        IamError.UsernameAlreadyTaken => StatusCodes.Status409Conflict,
        IamError.InvalidRole => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    public static IActionResult ToSignUpSuccessActionResult(ControllerBase controller) =>
        controller.StatusCode(StatusCodes.Status201Created);

    public static IActionResult ToSignInSuccessActionResult(User user, string token, ControllerBase controller) =>
        controller.Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(user, token));

    public static IActionResult ToFailureActionResult<T>(
        Result<T> result,
        ControllerBase controller,
        ProblemDetailsFactory factory) =>
        factory.CreateProblemDetails(controller, MapErrorToStatusCode(result.Error!), result.Error, result.Message);
}
