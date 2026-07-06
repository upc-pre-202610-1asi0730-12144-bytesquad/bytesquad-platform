using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Interfaces.REST.Transform;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;

namespace SpotTrack.Platform.Analytics.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
public class ActivityReportsController : ControllerBase
{
    private readonly IActivityReportCommandService _activityReportCommandService;

    public ActivityReportsController(IActivityReportCommandService activityReportCommandService)
    {
        _activityReportCommandService = activityReportCommandService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivityReport([FromBody] RequestActivityAnalysisCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var activityReport = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (activityReport == null) return BadRequest();

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(activityReport);
        return StatusCode(201, resource);
    }

    [HttpPost("total-usage-time")]
    public async Task<IActionResult> UpdateTotalUsageTime([FromBody] RequestTotalUsageTimeCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var activityReport = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (activityReport == null) return NotFound();

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(activityReport);
        return Ok(resource);
    }

    [HttpPost("downtime-cost")]
    public async Task<IActionResult> UpdateDowntimeCost([FromBody] RequestDowntimeCostCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var activityReport = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (activityReport == null) return NotFound();

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(activityReport);
        return Ok(resource);
    }

    [HttpPost("percentage-comparison")]
    public async Task<IActionResult> UpdatePercentageComparison([FromBody] RequestPercentageComparisonCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var activityReport = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (activityReport == null) return NotFound();

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(activityReport);
        return Ok(resource);
    }
}