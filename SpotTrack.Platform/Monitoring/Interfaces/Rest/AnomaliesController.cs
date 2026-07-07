using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
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
[Route("api/v1/anomalies")]
[Produces(MediaTypeNames.Application.Json)]
[AllowAnonymous]
[SwaggerTag("Anomaly reporting endpoints")]
public class AnomaliesController(
    IAnomalyCommandService anomalyCommandService,
    IAnomalyQueryService anomalyQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Report a detected anomaly",
        Description = "Records an anomaly detected by a sensor or external monitoring system.",
        OperationId = "ReportAnomaly")]
    [SwaggerResponse(StatusCodes.Status201Created, "Anomaly reported", typeof(AnomalyResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Sensor not found")]
    public async Task<IActionResult> ReportAnomaly(
        [FromBody] ReportAnomalyResource resource,
        CancellationToken cancellationToken)
    {
        var command = new ReportAnomalyCommand(resource.SensorId, resource.AnomalyType, resource.Description, resource.DetectedAt);
        var result = await anomalyCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!, AnomalyResourceFromEntityAssembler.ToResourceFromEntity, StatusCodes.Status201Created, this);
    }

    [HttpGet("me")]
    [Authorize(UserRole.Admin)]
    [SwaggerOperation(
        Summary = "Get anomalies for the authenticated admin",
        Description = "Returns all anomalies associated with sensors registered by the authenticated admin.",
        OperationId = "GetMyAnomalies")]
    [SwaggerResponse(StatusCodes.Status200OK, "Anomalies retrieved", typeof(IEnumerable<AnomalyResource>))]
    public async Task<IActionResult> GetMyAnomalies(CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var query = new GetAnomaliesByAdminIdQuery(adminId);
        var anomalies = await anomalyQueryService.Handle(query, cancellationToken);
        return Ok(anomalies.Select(AnomalyResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
