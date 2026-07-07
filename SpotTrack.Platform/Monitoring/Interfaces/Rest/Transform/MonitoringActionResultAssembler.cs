using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;

public static class MonitoringActionResultAssembler
{
    private static int MapErrorToStatusCode(Enum error) => error switch
    {
        MonitoringError.InvalidAnomalyData or MonitoringError.InvalidSensorData
            or MonitoringError.InvalidSensorStatus => StatusCodes.Status400BadRequest,
        MonitoringError.SensorNotFound             => StatusCodes.Status404NotFound,
        _                                          => StatusCodes.Status500InternalServerError
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
