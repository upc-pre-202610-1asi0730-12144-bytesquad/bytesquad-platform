using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Application.QueryServices;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Model.Queries;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/sensors")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
[SwaggerTag("IoT sensor connectivity endpoints")]
public class SensorsController(
    ISensorCommandService sensorCommandService,
    ISensorQueryService sensorQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Register a sensor",
        Description = "Registers a new IoT sensor linked to a piece of equipment.",
        OperationId = "RegisterSensor")]
    [SwaggerResponse(StatusCodes.Status201Created, "Sensor registered successfully", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid sensor data provided")]
    public async Task<IActionResult> RegisterSensor(
        [FromBody] RegisterSensorResource resource,
        CancellationToken cancellationToken)
    {
        var command = RegisterSensorCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            SensorResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get my sensors",
        Description = "Returns the sensors linked to equipment owned by the currently authenticated admin.",
        OperationId = "GetMySensors")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of sensors", typeof(IEnumerable<SensorResource>))]
    public async Task<IActionResult> GetMySensors(CancellationToken cancellationToken)
    {
        var adminUserId = ((User)HttpContext.Items["User"]!).Id;
        var sensors = await sensorQueryService.Handle(new GetSensorsByAdminIdQuery(adminUserId), cancellationToken);
        return Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPatch("{sensorId:int}/disconnect")]
    [SwaggerOperation(
        Summary = "Mark a sensor as disconnected",
        Description = "Simulates a network disconnection for the given sensor.",
        OperationId = "MarkSensorDisconnected")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sensor marked as disconnected", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sensor not found")]
    public async Task<IActionResult> MarkSensorDisconnected(
        [FromRoute] int sensorId,
        CancellationToken cancellationToken)
    {
        var result = await sensorCommandService.Handle(new MarkSensorDisconnectedCommand(sensorId), cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }

    [HttpPatch("{sensorId:int}/reconnect")]
    [SwaggerOperation(
        Summary = "Mark a sensor as reconnected",
        Description = "Simulates a network reconnection for the given sensor and syncs its last heartbeat.",
        OperationId = "MarkSensorReconnected")]
    [SwaggerResponse(StatusCodes.Status200OK, "Sensor marked as reconnected", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sensor not found")]
    public async Task<IActionResult> MarkSensorReconnected(
        [FromRoute] int sensorId,
        CancellationToken cancellationToken)
    {
        var result = await sensorCommandService.Handle(new MarkSensorReconnectedCommand(sensorId), cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return Ok(SensorResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }
}
