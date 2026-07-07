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
[Route("api/v1/motion-sensors")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Motion sensor management endpoints")]
public class MotionSensorsController(
    ISensorCommandService sensorCommandService,
    ISensorQueryService sensorQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Register a motion sensor",
        Description = "Registers a new motion sensor for the authenticated admin. AdminId is taken from JWT.",
        OperationId = "RegisterMotionSensor")]
    [SwaggerResponse(StatusCodes.Status201Created, "Motion sensor registered", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid sensor data")]
    public async Task<IActionResult> RegisterMotionSensor(
        [FromBody] RegisterSensorResource resource,
        CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var command = new RegisterSensorCommand(adminId, SensorType.MotionSensor, resource.Identifier, resource.EquipmentId);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!, SensorResourceFromEntityAssembler.ToResourceFromEntity, StatusCodes.Status201Created, this);
    }

    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get all motion sensors",
        Description = "Returns a list of all registered motion sensors.",
        OperationId = "GetAllMotionSensors")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of motion sensors", typeof(IEnumerable<SensorResource>))]
    public async Task<IActionResult> GetAllMotionSensors(CancellationToken cancellationToken)
    {
        var sensors = await sensorQueryService.Handle(new GetAllSensorsByTypeQuery(SensorType.MotionSensor), cancellationToken);
        return Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("me")]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Get motion sensors for the authenticated admin",
        Description = "Returns all motion sensors belonging to the authenticated admin.",
        OperationId = "GetMyMotionSensors")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of motion sensors", typeof(IEnumerable<SensorResource>))]
    public async Task<IActionResult> GetMyMotionSensors(CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var sensors = await sensorQueryService.Handle(
            new GetSensorsByAdminIdAndTypeQuery(adminId, SensorType.MotionSensor), cancellationToken);
        return Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("capture-motion")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Capture a motion detection event",
        Description = "Records a motion detection event reported by an IoT motion sensor device.",
        OperationId = "CaptureMotionEvent")]
    [SwaggerResponse(StatusCodes.Status200OK, "Motion event recorded", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sensor not found")]
    public async Task<IActionResult> CaptureMotionEvent(
        [FromBody] CaptureSensorEventResource resource,
        CancellationToken cancellationToken)
    {
        var command = new CaptureSensorEventCommand(resource.SensorId, resource.DetectedAt);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!, SensorResourceFromEntityAssembler.ToResourceFromEntity, StatusCodes.Status200OK, this);
    }
}
