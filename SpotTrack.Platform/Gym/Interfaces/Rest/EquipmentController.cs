using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Interfaces.Acl;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest;

[ApiController]
[Route("api/v1/equipment")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
[SwaggerTag("Equipment management endpoints")]
public class EquipmentController(
    IEquipmentCommandService equipmentCommandService,
    IEquipmentQueryService equipmentQueryService,
    IGymContextFacade gymContextFacade,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("{equipmentId:int}")]
    [SwaggerOperation(
        Summary = "Get equipment by ID",
        OperationId = "GetEquipmentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Equipment found", typeof(EquipmentResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Equipment not found")]
    public async Task<IActionResult> GetEquipmentById(
        [FromRoute] int equipmentId,
        CancellationToken cancellationToken)
    {
        var authenticatedAdminId = ((User)HttpContext.Items["User"]!).Id;
        var ownerAdminId = await gymContextFacade.GetAdminIdByEquipmentIdAsync(equipmentId, cancellationToken);
        if (ownerAdminId != authenticatedAdminId) return Forbid();

        var equipment = await equipmentQueryService.Handle(new GetEquipmentByIdQuery(equipmentId), cancellationToken);
        if (equipment is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, null, "Equipment not found.");
        return Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(equipment));
    }

    [HttpPatch("{equipmentId:int}/out-of-service")]
    [SwaggerOperation(
        Summary = "Mark equipment as out of service",
        OperationId = "MarkEquipmentOutOfService")]
    [SwaggerResponse(StatusCodes.Status200OK, "Equipment marked out of service", typeof(EquipmentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Equipment cannot be marked out of service in its current status")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Equipment not found")]
    public async Task<IActionResult> MarkEquipmentOutOfService(
        [FromRoute] int equipmentId,
        CancellationToken cancellationToken)
    {
        var authenticatedAdminId = ((User)HttpContext.Items["User"]!).Id;
        var ownerAdminId = await gymContextFacade.GetAdminIdByEquipmentIdAsync(equipmentId, cancellationToken);
        if (ownerAdminId != authenticatedAdminId) return Forbid();

        var command = new MarkEquipmentOutOfServiceCommand(equipmentId);
        var result = await equipmentCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return EquipmentActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return EquipmentActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            EquipmentResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpGet("by-admin/{adminId:int}")]
    [SwaggerOperation(
        Summary = "Get equipment by admin",
        Description = "Returns all equipment belonging to the authenticated admin's gym.",
        OperationId = "GetEquipmentByAdmin")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of equipment", typeof(IEnumerable<EquipmentResource>))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    public async Task<IActionResult> GetEquipmentByAdmin(
        [FromRoute] int adminId,
        CancellationToken cancellationToken)
    {
        var authenticatedAdminId = ((User)HttpContext.Items["User"]!).Id;
        if (adminId != authenticatedAdminId) return Forbid();

        var equipment = await equipmentQueryService.Handle(
            new GetEquipmentByAdminIdQuery(adminId), cancellationToken);
        return Ok(equipment.Select(EquipmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

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
