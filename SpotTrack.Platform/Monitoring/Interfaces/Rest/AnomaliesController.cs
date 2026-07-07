using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/anomalies")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Anomaly reporting endpoints")]
public class AnomaliesController(
    IAnomalyCommandService anomalyCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
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
}
