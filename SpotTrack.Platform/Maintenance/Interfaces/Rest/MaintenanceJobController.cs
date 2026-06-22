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
[Route("api/v1/maintenance-jobs")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Maintenance job management endpoints")]
public class MaintenanceJobController(
    IMaintenanceJobCommandService maintenanceJobCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("accept")]
    [SwaggerOperation(
        Summary = "Accept a maintenance job",
        Description = "Creates a new maintenance job for a technician accepting a technical ticket.",
        OperationId = "AcceptMaintenance")]
    [SwaggerResponse(StatusCodes.Status201Created, "Maintenance job accepted successfully",
        typeof(MaintenanceJobResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Technical ticket not found")]
    public async Task<IActionResult> AcceptMaintenance(
        [FromBody] AcceptMaintenanceResource resource,
        CancellationToken cancellationToken)
    {
        var command = AcceptMaintenanceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await maintenanceJobCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return MaintenanceJobActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return MaintenanceJobActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            MaintenanceJobResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
