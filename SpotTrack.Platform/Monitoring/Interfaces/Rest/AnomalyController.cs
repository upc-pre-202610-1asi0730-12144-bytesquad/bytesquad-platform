using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Monitoring.Application.CommandServices;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/anomalies")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
[SwaggerTag("Anomaly reporting endpoints")]
public class AnomalyController(
    IAnomalyCommandService anomalyCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Report an anomaly",
        Description = "Reports a new equipment anomaly detected during a reservation.",
        OperationId = "ReportAnomaly")]
    [SwaggerResponse(StatusCodes.Status201Created, "Anomaly reported successfully", typeof(AnomalyResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    public async Task<IActionResult> ReportAnomaly(
        [FromBody] ReportAnomalyResource resource,
        CancellationToken cancellationToken)
    {
        var command = ReportAnomalyCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await anomalyCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return MonitoringActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return MonitoringActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            AnomalyResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
