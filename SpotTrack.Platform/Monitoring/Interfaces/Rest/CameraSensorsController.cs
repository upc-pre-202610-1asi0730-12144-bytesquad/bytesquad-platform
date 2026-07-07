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
[Route("api/v1/camera-sensors")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Camera sensor management endpoints")]
public class CameraSensorsController(
    ISensorCommandService sensorCommandService,
    ISensorQueryService sensorQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Register a camera sensor",
        Description = "Registers a new camera sensor for the authenticated admin. AdminId is taken from JWT.",
        OperationId = "RegisterCameraSensor")]
    [SwaggerResponse(StatusCodes.Status201Created, "Camera sensor registered", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid sensor data")]
    public async Task<IActionResult> RegisterCameraSensor(
        [FromBody] RegisterSensorResource resource,
        CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var command = new RegisterSensorCommand(adminId, SensorType.CameraSensor, resource.Identifier, resource.EquipmentId);
        var result = await sensorCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!, SensorResourceFromEntityAssembler.ToResourceFromEntity, StatusCodes.Status201Created, this);
    }

    [HttpGet]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Get all camera sensors",
        Description = "Returns a list of all registered camera sensors.",
        OperationId = "GetAllCameraSensors")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of camera sensors", typeof(IEnumerable<SensorResource>))]
    public async Task<IActionResult> GetAllCameraSensors(CancellationToken cancellationToken)
    {
        var sensors = await sensorQueryService.Handle(new GetAllSensorsByTypeQuery(SensorType.CameraSensor), cancellationToken);
        return Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("me")]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Get camera sensors for the authenticated admin",
        Description = "Returns all camera sensors belonging to the authenticated admin.",
        OperationId = "GetMyCameraSensors")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of camera sensors", typeof(IEnumerable<SensorResource>))]
    public async Task<IActionResult> GetMyCameraSensors(CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var sensors = await sensorQueryService.Handle(
            new GetSensorsByAdminIdAndTypeQuery(adminId, SensorType.CameraSensor), cancellationToken);
        return Ok(sensors.Select(SensorResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost("capture")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Capture a camera detection event",
        Description = "Records a detection event reported by an IoT camera sensor device.",
        OperationId = "CaptureCameraEvent")]
    [SwaggerResponse(StatusCodes.Status200OK, "Camera event recorded", typeof(SensorResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sensor not found")]
    public async Task<IActionResult> CaptureCameraEvent(
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
