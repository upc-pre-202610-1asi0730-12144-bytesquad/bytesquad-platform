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
[Route("api/v1/routine-sessions")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Routine session management endpoints")]
public class RoutineSessionsController(
    IRoutineSessionCommandService routineSessionCommandService,
    IRoutineSessionQueryService routineSessionQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Start a routine session",
        Description = "Starts a new routine session for the given client and routine. Returns 400 if the data is invalid.",
        OperationId = "StartRoutine")]
    [SwaggerResponse(StatusCodes.Status201Created, "Routine session started successfully", typeof(RoutineSessionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid session data provided")]
    public async Task<IActionResult> StartRoutine(
        [FromBody] StartRoutineResource resource,
        CancellationToken cancellationToken)
    {
        var command = StartRoutineCommandFromResourceAssembler.ToCommandFromResource(resource);
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
        Description = "Returns the routine session matching the given ID, or 404 if not found.",
        OperationId = "GetRoutineSessionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Routine session found", typeof(RoutineSessionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Routine session not found")]
    public async Task<IActionResult> GetRoutineSessionById(
        [FromRoute] int routineSessionId,
        CancellationToken cancellationToken)
    {
        var session = await routineSessionQueryService.Handle(
            new GetRoutineSessionByIdQuery(routineSessionId), cancellationToken);
        if (session is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound,
                RoutinesError.RoutineSessionNotFound, "Routine session not found.");
        return Ok(RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity(session));
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all routine sessions by client ID",
        Description = "Returns the list of routine sessions belonging to the given client.",
        OperationId = "GetAllRoutineSessionsByClientId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of routine sessions", typeof(IEnumerable<RoutineSessionResource>))]
    public async Task<IActionResult> GetAllRoutineSessionsByClientId(
        [FromQuery] int clientId,
        CancellationToken cancellationToken)
    {
        var sessions = await routineSessionQueryService.Handle(
            new GetAllRoutineSessionsByClientIdQuery(clientId), cancellationToken);
        var resources = sessions.Select(RoutineSessionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
