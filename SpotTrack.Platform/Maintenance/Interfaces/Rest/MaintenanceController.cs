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
[Route("api/v1/maintenances")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Maintenance management endpoints")]
public class MaintenanceController(
    IMaintenanceCommandService maintenanceCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("request")]
    [SwaggerOperation(
        Summary = "Request maintenance for equipment",
        Description = "Creates a new maintenance request for a given equipment item.",
        OperationId = "RequestMaintenance")]
    [SwaggerResponse(StatusCodes.Status201Created, "Maintenance request created successfully",
        typeof(MaintenanceResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid maintenance data provided")]
    public async Task<IActionResult> RequestMaintenance(
        [FromBody] RequestMaintenanceResource resource,
        CancellationToken cancellationToken)
    {
        var command = RequestMaintenanceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await maintenanceCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return MaintenanceActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return MaintenanceActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            MaintenanceResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
