using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using SpotTrack.Platform.Profiles.Interfaces.Acl;
using SpotTrack.Platform.Routines.Application.CommandServices;
using SpotTrack.Platform.Routines.Application.QueryServices;
using SpotTrack.Platform.Routines.Domain.Model;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Queries;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;
using SpotTrack.Platform.Routines.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Routines.Interfaces.Rest;

[ApiController]
[Route("api/v1/routines")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Client)]
[SwaggerTag("Routine management endpoints")]
public class RoutinesController(
    IRoutineCommandService routineCommandService,
    IRoutineQueryService routineQueryService,
    IProfilesContextFacade profilesContextFacade,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new routine",
        Description = "Creates a new routine for the currently authenticated client. Returns 400 if the data is invalid.",
        OperationId = "CreateRoutine")]
    [SwaggerResponse(StatusCodes.Status201Created, "Routine created successfully", typeof(RoutineResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid routine data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> CreateRoutine(
        [FromBody] CreateRoutineResource resource,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var command = CreateRoutineCommandFromResourceAssembler.ToCommandFromResource(clientId.Value, resource);
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
        Description = "Adds a new exercise block to an existing routine owned by the authenticated client.",
        OperationId = "AddExerciseBlock")]
    [SwaggerResponse(StatusCodes.Status201Created, "Exercise block added successfully", typeof(ExerciseBlockResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid exercise data provided")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine not found")]
    public async Task<IActionResult> AddExerciseBlock(
        [FromRoute] int routineId,
        [FromBody] AddExerciseBlockResource resource,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var routine = await routineQueryService.Handle(new GetRoutineByIdQuery(routineId), cancellationToken);
        if (routine is null)
            return RoutineNotFound();

        var ownershipError = CheckOwnership(routine, clientId.Value);
        if (ownershipError is not null)
            return ownershipError;

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

    [HttpGet("{routineId:int}/exercise-blocks")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Get exercise blocks for a routine",
        OperationId = "GetExerciseBlocksByRoutineId")]
    [SwaggerResponse(StatusCodes.Status200OK, "Exercise blocks retrieved successfully",
        typeof(IEnumerable<ExerciseBlockResource>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine not found")]
    public async Task<IActionResult> GetExerciseBlocksByRoutineId(
        [FromRoute] int routineId,
        CancellationToken cancellationToken)
    {
        var routine = await routineQueryService.Handle(new GetRoutineByIdQuery(routineId), cancellationToken);
        if (routine is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, RoutinesError.RoutineNotFound, "Routine not found.");
        return Ok(routine.ExerciseBlocks.Select(ExerciseBlockResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{routineId:int}")]
    [SwaggerOperation(
        Summary = "Get a routine by ID",
        Description = "Returns the routine matching the given ID, if it belongs to the authenticated client.",
        OperationId = "GetRoutineById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Routine found", typeof(RoutineResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine not found")]
    public async Task<IActionResult> GetRoutineById(
        [FromRoute] int routineId,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var routine = await routineQueryService.Handle(new GetRoutineByIdQuery(routineId), cancellationToken);
        if (routine is null)
            return RoutineNotFound();

        var ownershipError = CheckOwnership(routine, clientId.Value);
        if (ownershipError is not null)
            return ownershipError;

        return Ok(RoutineResourceFromEntityAssembler.ToResourceFromEntity(routine));
    }

    [HttpGet("{routineId:int}/exercise-blocks")]
    [SwaggerOperation(
        Summary = "Get exercise blocks for a routine",
        Description = "Returns the exercise blocks belonging to a routine owned by the authenticated client.",
        OperationId = "GetExerciseBlocksByRoutine")]
    [SwaggerResponse(StatusCodes.Status200OK, "Exercise blocks retrieved successfully", typeof(IEnumerable<ExerciseBlockResource>))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine not found")]
    public async Task<IActionResult> GetExerciseBlocksByRoutine(
        [FromRoute] int routineId,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var routine = await routineQueryService.Handle(new GetRoutineByIdQuery(routineId), cancellationToken);
        if (routine is null)
            return RoutineNotFound();

        var ownershipError = CheckOwnership(routine, clientId.Value);
        if (ownershipError is not null)
            return ownershipError;

        var resources = routine.ExerciseBlocks.Select(ExerciseBlockResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all my routines",
        Description = "Returns the list of routines belonging to the currently authenticated client.",
        OperationId = "GetAllRoutines")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of routines", typeof(IEnumerable<RoutineResource>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> GetAllRoutines(CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var routines = await routineQueryService.Handle(
            new GetAllRoutinesByClientIdQuery(clientId.Value), cancellationToken);
        var resources = routines.Select(RoutineResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    private async Task<int?> ResolveClientIdAsync()
    {
        var userId = HttpContext.GetAuthenticatedUserId()!.Value;
        var clientId = await profilesContextFacade.FetchClientIdByUserIdAsync(userId);
        return clientId == 0 ? null : clientId;
    }

    private IActionResult? CheckOwnership(Routine routine, int clientId) =>
        routine.ClientId.Value != clientId
            ? problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status403Forbidden, RoutinesError.AccessDenied, "Access denied.")
            : null;

    private IActionResult ClientNotFound() =>
        problemDetailsFactory.CreateProblemDetails(
            this, StatusCodes.Status404NotFound, RoutinesError.ClientNotFound, "Client not found.");

    private IActionResult RoutineNotFound() =>
        problemDetailsFactory.CreateProblemDetails(
            this, StatusCodes.Status404NotFound, RoutinesError.RoutineNotFound, "Routine not found.");
}
