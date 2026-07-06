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
using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Domain.Model.Queries;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;
using SpotTrack.Platform.Routines.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Routines.Interfaces.Rest;

[ApiController]
[Route("api/v1/routine-sessions")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Client)]
[SwaggerTag("Routine session management endpoints")]
public class RoutineSessionsController(
    IRoutineSessionCommandService routineSessionCommandService,
    IRoutineSessionQueryService routineSessionQueryService,
    IProfilesContextFacade profilesContextFacade,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Start a routine session",
        Description = "Starts a new routine session for the currently authenticated client. Returns 400 if the data is invalid.",
        OperationId = "StartRoutine")]
    [SwaggerResponse(StatusCodes.Status201Created, "Routine session started successfully", typeof(RoutineSessionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid session data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> StartRoutine(
        [FromBody] StartRoutineResource resource,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var command = StartRoutineCommandFromResourceAssembler.ToCommandFromResource(clientId.Value, resource);
        var result = await routineSessionCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return RoutinesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return RoutinesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpGet("{routineSessionId:int}")]
    [SwaggerOperation(
        Summary = "Get a routine session by ID",
        Description = "Returns the routine session matching the given ID, if it belongs to the authenticated client.",
        OperationId = "GetRoutineSessionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Routine session found", typeof(RoutineSessionResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine session not found")]
    public async Task<IActionResult> GetRoutineSessionById(
        [FromRoute] int routineSessionId,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var session = await routineSessionQueryService.Handle(
            new GetRoutineSessionByIdQuery(routineSessionId), cancellationToken);
        if (session is null)
            return SessionNotFound();

        var ownershipError = CheckOwnership(session, clientId.Value);
        if (ownershipError is not null)
            return ownershipError;

        return Ok(RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity(session));
    }

    [HttpPost("{routineSessionId:int}/completions")]
    [SwaggerOperation(
        Summary = "Complete a routine session",
        Description = "Marks the given routine session as completed, if it belongs to the authenticated client.",
        OperationId = "CompleteRoutine")]
    [SwaggerResponse(StatusCodes.Status200OK, "Routine session completed successfully", typeof(RoutineSessionResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine session not found")]
    public async Task<IActionResult> CompleteRoutine(
        [FromRoute] int routineSessionId,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var session = await routineSessionQueryService.Handle(
            new GetRoutineSessionByIdQuery(routineSessionId), cancellationToken);
        if (session is null)
            return SessionNotFound();

        var ownershipError = CheckOwnership(session, clientId.Value);
        if (ownershipError is not null)
            return ownershipError;

        var command = new CompleteRoutineCommand(routineSessionId);
        var result = await routineSessionCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return RoutinesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return RoutinesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpPost("{routineSessionId:int}/missed")]
    [SwaggerOperation(
        Summary = "Mark a routine session as missed",
        Description = "Marks the given routine session as missed, if it belongs to the authenticated client.",
        OperationId = "MarkRoutineMissed")]
    [SwaggerResponse(StatusCodes.Status200OK, "Routine session marked as missed successfully", typeof(RoutineSessionResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine session not found")]
    public async Task<IActionResult> MarkRoutineMissed(
        [FromRoute] int routineSessionId,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var session = await routineSessionQueryService.Handle(
            new GetRoutineSessionByIdQuery(routineSessionId), cancellationToken);
        if (session is null)
            return SessionNotFound();

        var ownershipError = CheckOwnership(session, clientId.Value);
        if (ownershipError is not null)
            return ownershipError;

        var command = new MarkRoutineMissedCommand(routineSessionId);
        var result = await routineSessionCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return RoutinesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return RoutinesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpPatch("{routineSessionId:int}/exercise-blocks/{exerciseBlockId:int}")]
    [SwaggerOperation(
        Summary = "Mark an exercise block as completed or not",
        Description = "Sets the completion state of an exercise block for this routine session, if it belongs to the authenticated client.",
        OperationId = "SetExerciseBlockCompletion")]
    [SwaggerResponse(StatusCodes.Status200OK, "Completion state updated successfully", typeof(RoutineSessionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Exercise block does not belong to this routine, or session is not active")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine session not found")]
    public async Task<IActionResult> SetExerciseBlockCompletion(
        [FromRoute] int routineSessionId,
        [FromRoute] int exerciseBlockId,
        [FromBody] SetExerciseBlockCompletionResource resource,
        CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var session = await routineSessionQueryService.Handle(
            new GetRoutineSessionByIdQuery(routineSessionId), cancellationToken);
        if (session is null)
            return SessionNotFound();

        var ownershipError = CheckOwnership(session, clientId.Value);
        if (ownershipError is not null)
            return ownershipError;

        var command = new SetExerciseBlockCompletionCommand(routineSessionId, exerciseBlockId, resource.Completed);
        var result = await routineSessionCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return RoutinesActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return RoutinesActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all my routine sessions",
        Description = "Returns the list of routine sessions belonging to the currently authenticated client.",
        OperationId = "GetAllRoutineSessions")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of routine sessions", typeof(IEnumerable<RoutineSessionResource>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Client profile not found")]
    public async Task<IActionResult> GetAllRoutineSessions(CancellationToken cancellationToken)
    {
        var clientId = await ResolveClientIdAsync();
        if (clientId is null)
            return ClientNotFound();

        var sessions = await routineSessionQueryService.Handle(
            new GetAllRoutineSessionsByClientIdQuery(clientId.Value), cancellationToken);
        var resources = sessions.Select(RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    private async Task<int?> ResolveClientIdAsync()
    {
        var userId = HttpContext.GetAuthenticatedUserId()!.Value;
        var clientId = await profilesContextFacade.FetchClientIdByUserIdAsync(userId);
        return clientId == 0 ? null : clientId;
    }

    private IActionResult? CheckOwnership(RoutineSession session, int clientId) =>
        session.ClientId.Value != clientId
            ? problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status403Forbidden, RoutinesError.AccessDenied, "Access denied.")
            : null;

    private IActionResult ClientNotFound() =>
        problemDetailsFactory.CreateProblemDetails(
            this, StatusCodes.Status404NotFound, RoutinesError.ClientNotFound, "Client not found.");

    private IActionResult SessionNotFound() =>
        problemDetailsFactory.CreateProblemDetails(
            this, StatusCodes.Status404NotFound,
            RoutinesError.RoutineSessionNotFound, "Routine session not found.");
}
