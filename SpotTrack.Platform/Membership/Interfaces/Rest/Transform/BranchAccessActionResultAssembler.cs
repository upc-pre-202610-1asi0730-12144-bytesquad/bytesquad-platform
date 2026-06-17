using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Memberships.Domain.Model;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Transform;

public static class BranchAccessActionResultAssembler
{
    private static int MapErrorToStatusCode(Enum error) => error switch
    {
        BranchAccessError.BranchAccessNotFound => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    };

    public static IActionResult ToSuccessActionResult<TEntity, TResource>(
        TEntity entity,
        Func<TEntity, TResource> toResource,
        int statusCode,
        ControllerBase controller) =>
        controller.StatusCode(statusCode, toResource(entity));

    public static IActionResult ToFailureActionResult<T>(
        Result<T> result,
        ControllerBase controller,
        ProblemDetailsFactory factory) =>
        factory.CreateProblemDetails(controller, MapErrorToStatusCode(result.Error!), result.Error, result.Message);
}
