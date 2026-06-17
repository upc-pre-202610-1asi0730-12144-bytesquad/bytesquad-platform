using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Routines.Application.CommandServices;
using SpotTrack.Platform.Routines.Application.QueryServices;
using SpotTrack.Platform.Routines.Domain.Model;
using SpotTrack.Platform.Routines.Domain.Model.Queries;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;
using SpotTrack.Platform.Routines.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Routines.Interfaces.Rest;

[ApiController]
[Route("api/v1/routines")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Routine management endpoints")]
public class RoutinesController(
    IRoutineCommandService routineCommandService,
    IRoutineQueryService routineQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new routine",
        Description = "Creates a new routine for the given client. Returns 400 if the data is invalid.",
        OperationId = "CreateRoutine")]
    [SwaggerResponse(StatusCodes.Status201Created, "Routine created successfully", typeof(RoutineResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid routine data provided")]
    public async Task<IActionResult> CreateRoutine(
        [FromBody] CreateRoutineResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateRoutineCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await routineCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return RoutinesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return RoutinesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            RoutineResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpPost("{routineId:int}/exercise-blocks")]
    [SwaggerOperation(
        Summary = "Add an exercise block to a routine",
        Description = "Adds a new exercise block to an existing routine. Returns 404 if the routine is not found, 400 if the exercise data is invalid.",
        OperationId = "AddExerciseBlock")]
    [SwaggerResponse(StatusCodes.Status201Created, "Exercise block added successfully", typeof(ExerciseBlockResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid exercise data provided")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine not found")]
    public async Task<IActionResult> AddExerciseBlock(
        [FromRoute] int routineId,
        [FromBody] AddExerciseBlockResource resource,
        CancellationToken cancellationToken)
    {
        var command = AddExerciseBlockCommandFromResourceAssembler.ToCommandFromResource(routineId, resource);
        var result = await routineCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return RoutinesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return RoutinesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ExerciseBlockResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpGet("{routineId:int}")]
    [SwaggerOperation(
        Summary = "Get a routine by ID",
        Description = "Returns the routine matching the given ID, or 404 if not found.",
        OperationId = "GetRoutineById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Routine found", typeof(RoutineResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine not found")]
    public async Task<IActionResult> GetRoutineById(
        [FromRoute] int routineId,
        CancellationToken cancellationToken)
    {
        var routine = await routineQueryService.Handle(new GetRoutineByIdQuery(routineId), cancellationToken);
        if (routine is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, RoutinesError.RoutineNotFound, "Routine not found.");
        return Ok(RoutineResourceFromEntityAssembler.ToResourceFromEntity(routine));
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all routines by client ID",
        Description = "Returns the list of routines belonging to the given client.",
        OperationId = "GetAllRoutinesByClientId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of routines", typeof(IEnumerable<RoutineResource>))]
    public async Task<IActionResult> GetAllRoutinesByClientId(
        [FromQuery] int clientId,
        CancellationToken cancellationToken)
    {
        var routines = await routineQueryService.Handle(new GetAllRoutinesByClientIdQuery(clientId), cancellationToken);
        var resources = routines.Select(RoutineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
