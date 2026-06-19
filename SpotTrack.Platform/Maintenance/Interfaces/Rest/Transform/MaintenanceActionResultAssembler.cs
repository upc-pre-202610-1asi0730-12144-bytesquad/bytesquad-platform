using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Maintenances.Domain.Model;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class MaintenanceActionResultAssembler
{
    private static int MapErrorToStatusCode(Enum error) => error switch
    {
        MaintenanceError.MaintenanceNotFound => StatusCodes.Status404NotFound,
        MaintenanceError.InvalidMaintenanceData or
            MaintenanceError.InvalidMaintenanceStatus => StatusCodes.Status400BadRequest,
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
