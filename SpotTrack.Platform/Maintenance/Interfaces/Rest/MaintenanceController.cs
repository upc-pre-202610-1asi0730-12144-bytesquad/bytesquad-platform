using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Maintenances.Application.CommandServices;
using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;
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
    IMaintenanceQueryService maintenanceQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("by-equipment/{equipmentId:int}")]
    [SwaggerOperation(
        Summary = "Get all maintenances by equipment id",
        OperationId = "GetAllMaintenancesByEquipmentId")]
    [SwaggerResponse(StatusCodes.Status200OK, "Maintenances retrieved successfully",
        typeof(IEnumerable<MaintenanceResource>))]
    public async Task<IActionResult> GetAllMaintenancesByEquipmentId(
        [FromRoute] int equipmentId,
        CancellationToken cancellationToken)
    {
        var maintenances = await maintenanceQueryService.Handle(
            new GetAllMaintenancesByEquipmentIdQuery(equipmentId), cancellationToken);
        return Ok(maintenances.Select(MaintenanceResourceFromEntityAssembler.ToResourceFromEntity));
    }

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
