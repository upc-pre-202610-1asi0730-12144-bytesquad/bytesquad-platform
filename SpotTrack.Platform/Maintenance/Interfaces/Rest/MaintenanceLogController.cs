using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Maintenances.Application.CommandServices;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest;

[ApiController]
[Route("api/v1/maintenance-logs")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Maintenance log management endpoints")]
public class MaintenanceLogController(
    IMaintenanceLogCommandService maintenanceLogCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Register a maintenance completion log",
        Description = "Creates an immutable log entry for a completed maintenance ticket.",
        OperationId = "RegisterMaintenanceCompletion")]
    [SwaggerResponse(StatusCodes.Status201Created, "Maintenance completion registered successfully",
        typeof(MaintenanceLogResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Technical ticket not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Technical ticket is not resolved")]
    public async Task<IActionResult> RegisterMaintenanceCompletion(
        [FromBody] RegisterMaintenanceCompletionResource resource,
        CancellationToken cancellationToken)
    {
        var command = RegisterMaintenanceCompletionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await maintenanceLogCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return MaintenanceLogActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return MaintenanceLogActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            MaintenanceLogResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
