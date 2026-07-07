using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Model;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/session-trackers")]
[Produces(MediaTypeNames.Application.Json)]
[AllowAnonymous]
[SwaggerTag("Equipment session tracker endpoints")]
public class SessionTrackersController(
    ISessionTrackerCommandService sessionTrackerCommandService,
    ISessionTrackerQueryService sessionTrackerQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Create a session tracker",
        Description = "Creates a new equipment usage session tracker for the authenticated admin's equipment.",
        OperationId = "CreateSessionTracker")]
    [SwaggerResponse(StatusCodes.Status201Created, "Session tracker created", typeof(SessionTrackerResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid tracker data")]
    public async Task<IActionResult> CreateSessionTracker(
        [FromBody] CreateSessionTrackerResource resource,
        CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var command = new CreateSessionTrackerCommand(resource.EquipmentId, adminId, resource.StartedAt);
        var result = await sessionTrackerCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!, SessionTrackerResourceFromEntityAssembler.ToResourceFromEntity, StatusCodes.Status201Created, this);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all session trackers",
        Description = "Returns all equipment usage session trackers.",
        OperationId = "GetAllSessionTrackers")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of session trackers", typeof(IEnumerable<SessionTrackerResource>))]
    public async Task<IActionResult> GetAllSessionTrackers(CancellationToken cancellationToken)
    {
        var trackers = await sessionTrackerQueryService.Handle(new GetAllSessionTrackersQuery(), cancellationToken);
        return Ok(trackers.Select(SessionTrackerResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("me")]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Get session trackers for the authenticated admin",
        Description = "Returns all session trackers belonging to the authenticated admin's equipment.",
        OperationId = "GetMySessionTrackers")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of session trackers", typeof(IEnumerable<SessionTrackerResource>))]
    public async Task<IActionResult> GetMySessionTrackers(CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var trackers = await sessionTrackerQueryService.Handle(
            new GetSessionTrackersByAdminIdQuery(adminId), cancellationToken);
        return Ok(trackers.Select(SessionTrackerResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}/verify")]
    [SwaggerOperation(
        Summary = "Verify if a session tracker is active",
        Description = "Returns whether the given session tracker is currently active.",
        OperationId = "VerifySessionTracker")]
    [SwaggerResponse(StatusCodes.Status200OK, "Session tracker status", typeof(SessionTrackerResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Session tracker not found")]
    public async Task<IActionResult> VerifySessionTracker(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var tracker = await sessionTrackerQueryService.Handle(new GetSessionTrackerByIdQuery(id), cancellationToken);
        if (tracker is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, MonitoringError.SessionTrackerNotFound, "Session tracker not found.");
        return Ok(SessionTrackerResourceFromEntityAssembler.ToResourceFromEntity(tracker));
    }

    [HttpPatch("{id:int}/end")]
    [SwaggerOperation(
        Summary = "End a session tracker",
        Description = "Marks the given session tracker as ended and records the end time.",
        OperationId = "EndSessionTracker")]
    [SwaggerResponse(StatusCodes.Status200OK, "Session tracker ended", typeof(SessionTrackerResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Session tracker not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Session tracker is already ended")]
    public async Task<IActionResult> EndSessionTracker(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new EndSessionTrackerCommand(id);
        var result = await sessionTrackerCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!, SessionTrackerResourceFromEntityAssembler.ToResourceFromEntity, StatusCodes.Status200OK, this);
    }

    [HttpGet("{id:int}/time")]
    [SwaggerOperation(
        Summary = "Get elapsed time for a session tracker",
        Description = "Returns the elapsed time in seconds since the session tracker was started.",
        OperationId = "GetSessionTrackerTime")]
    [SwaggerResponse(StatusCodes.Status200OK, "Elapsed time", typeof(SessionTrackerTimeResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Session tracker not found")]
    public async Task<IActionResult> GetSessionTrackerTime(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var tracker = await sessionTrackerQueryService.Handle(new GetSessionTrackerByIdQuery(id), cancellationToken);
        if (tracker is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, MonitoringError.SessionTrackerNotFound, "Session tracker not found.");
        return Ok(SessionTrackerResourceFromEntityAssembler.ToTimeResourceFromEntity(tracker));
    }
}
