using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest;

[ApiController]
[Route("api/v1/equipment")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
[SwaggerTag("Equipment management endpoints")]
public class EquipmentController(
    IEquipmentCommandService equipmentCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Register new equipment",
        Description = "Registers new equipment in a zone. Requires Admin authentication. Returns 404 if the zone does not exist.",
        OperationId = "RegisterEquipment")]
    [SwaggerResponse(StatusCodes.Status201Created, "Equipment registered successfully", typeof(EquipmentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid equipment data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Zone not found")]
    public async Task<IActionResult> RegisterEquipment(
        [FromBody] RegisterEquipmentResource resource,
        CancellationToken cancellationToken)
    {
        var command = RegisterEquipmentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await equipmentCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return EquipmentActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return EquipmentActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            EquipmentResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
