using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Alerts.Application.CommandServices;
using SpotTrack.Platform.Alerts.Application.QueryServices;
using SpotTrack.Platform.Alerts.Domain.Model;
using SpotTrack.Platform.Alerts.Domain.Model.Commands;
using SpotTrack.Platform.Alerts.Domain.Model.Queries;
using SpotTrack.Platform.Alerts.Interfaces.Rest.Resources;
using SpotTrack.Platform.Alerts.Interfaces.Rest.Transform;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Alerts.Interfaces.Rest;

[ApiController]
[Route("api/v1/alerts")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
[SwaggerTag("Alert management endpoints")]
public class AlertsController(
    IAlertCommandService alertCommandService,
    IAlertQueryService alertQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all alerts",
        Description = "Returns all alerts belonging to the authenticated admin.",
        OperationId = "GetMyAlerts")]
    [SwaggerResponse(StatusCodes.Status200OK, "Alerts retrieved", typeof(IEnumerable<AlertResource>))]
    public async Task<IActionResult> GetMyAlerts(CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var alerts = await alertQueryService.Handle(new GetAlertsByAdminIdQuery(adminId), cancellationToken);
        return Ok(alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPatch("{alertId:int}/resolve")]
    [SwaggerOperation(
        Summary = "Resolve an alert",
        Description = "Marks an alert as resolved. Returns 403 if the alert does not belong to the authenticated admin.",
        OperationId = "ResolveAlert")]
    [SwaggerResponse(StatusCodes.Status200OK, "Alert resolved", typeof(AlertResource))]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "Access denied")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Alert not found")]
    public async Task<IActionResult> ResolveAlert(
        [FromRoute] int alertId,
        CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await alertCommandService.Handle(new ResolveAlertCommand(alertId, adminId), cancellationToken);
        if (result.IsFailure)
        {
            var statusCode = result.Error switch
            {
                AlertError.AccessDenied => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status404NotFound
            };
            return problemDetailsFactory.CreateProblemDetails(this, statusCode, result.Error!, result.Message);
        }
        return Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(result.Value!));
    }
}
