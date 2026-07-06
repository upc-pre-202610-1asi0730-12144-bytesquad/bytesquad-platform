using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Gyms.Domain.Model;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Model.Queries;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest;

[ApiController]
[Route("api/v1/gyms")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
[SwaggerTag("Gym management endpoints")]
public class GymsController(
    IGymCommandService gymCommandService,
    IGymQueryService gymQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet("{gymId:int}/branches")]
    [SwaggerOperation(
        Summary = "Get branches by gym id",
        Description = "Returns the branches of the given gym. Returns 404 if the gym is not found.",
        OperationId = "GetBranchesByGymId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of branches", typeof(IEnumerable<BranchResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Gym not found")]
    public async Task<IActionResult> GetBranchesByGymId(
        [FromRoute] int gymId,
        CancellationToken cancellationToken)
    {
        var branches = await gymQueryService.Handle(new GetBranchesByGymIdQuery(gymId), cancellationToken);
        if (branches is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, GymError.GymNotFound, "Gym not found.");
        return Ok(branches.Select(BranchResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{gymId:int}/zones")]
    [SwaggerOperation(
        Summary = "Get zones by gym id",
        Description = "Returns the zones across all branches of the given gym. Returns 404 if the gym is not found.",
        OperationId = "GetZonesByGymId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of zones", typeof(IEnumerable<ZoneResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Gym not found")]
    public async Task<IActionResult> GetZonesByGymId(
        [FromRoute] int gymId,
        CancellationToken cancellationToken)
    {
        var zones = await gymQueryService.Handle(new GetZonesByGymIdQuery(gymId), cancellationToken);
        if (zones is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, GymError.GymNotFound, "Gym not found.");
        return Ok(zones.Select(ZoneResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{gymId:int}/equipments")]
    [SwaggerOperation(
        Summary = "Get equipments by gym id",
        Description = "Returns the equipment across all zones of the given gym. Returns 404 if the gym is not found.",
        OperationId = "GetEquipmentsByGymId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of equipment", typeof(IEnumerable<EquipmentResource>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Gym not found")]
    public async Task<IActionResult> GetEquipmentsByGymId(
        [FromRoute] int gymId,
        CancellationToken cancellationToken)
    {
        var equipment = await gymQueryService.Handle(new GetEquipmentsByGymIdQuery(gymId), cancellationToken);
        if (equipment is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, GymError.GymNotFound, "Gym not found.");
        return Ok(equipment.Select(EquipmentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new gym",
        Description = "Creates a new gym with the given name and address. Requires Admin authentication.",
        OperationId = "CreateGym")]
    [SwaggerResponse(StatusCodes.Status201Created, "Gym created successfully", typeof(GymResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid gym data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    public async Task<IActionResult> CreateGym(
        [FromBody] CreateGymResource resource,
        CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var command = CreateGymCommandFromResourceAssembler.ToCommandFromResource(adminId, resource);
        var result = await gymCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return GymsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return GymsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            GymResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpPost("{gymId:int}/branches")]
    [SwaggerOperation(
        Summary = "Add a branch to a gym",
        Description = "Adds a new branch to an existing gym. Returns 404 if the gym is not found, 400 if the branch data is invalid.",
        OperationId = "CreateBranch")]
    [SwaggerResponse(StatusCodes.Status201Created, "Branch created successfully", typeof(BranchResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid branch data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Gym not found")]
    public async Task<IActionResult> CreateBranch(
        [FromRoute] int gymId,
        [FromBody] CreateBranchResource resource,
        CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var command = CreateBranchCommandFromResourceAssembler.ToCommandFromResource(gymId, adminId, resource);
        var result = await gymCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return GymsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return GymsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            BranchResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpPost("{gymId:int}/branches/{branchId:int}/zones")]
    [SwaggerOperation(
        Summary = "Add a zone to a branch",
        Description = "Adds a new zone to an existing branch within a gym. Returns 404 if the gym or branch is not found, 400 if the zone data is invalid.",
        OperationId = "CreateZone")]
    [SwaggerResponse(StatusCodes.Status201Created, "Zone created successfully", typeof(ZoneResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid zone data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Gym or branch not found")]
    public async Task<IActionResult> CreateZone(
        [FromRoute] int gymId,
        [FromRoute] int branchId,
        [FromBody] CreateZoneResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateZoneCommandFromResourceAssembler.ToCommandFromResource(gymId, branchId, resource);
        var result = await gymCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return GymsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return GymsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ZoneResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
