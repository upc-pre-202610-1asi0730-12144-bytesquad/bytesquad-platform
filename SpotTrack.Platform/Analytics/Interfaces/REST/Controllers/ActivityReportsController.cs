using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Application.QueryServices;
using SpotTrack.Platform.Analytics.Domain.Model;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.Queries;
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
    private readonly IActivityReportQueryService _activityReportQueryService;

    public ActivityReportsController(
        IActivityReportCommandService activityReportCommandService,
        IActivityReportQueryService activityReportQueryService)
    {
        _activityReportCommandService = activityReportCommandService;
        _activityReportQueryService = activityReportQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivityReport([FromBody] RequestActivityAnalysisCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure) return BadRequest(result.Message);

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return StatusCode(201, resource);
    }

    [HttpPost("total-usage-time")]
    public async Task<IActionResult> UpdateTotalUsageTime([FromBody] RequestTotalUsageTimeCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }

    [HttpPost("downtime-cost")]
    public async Task<IActionResult> UpdateDowntimeCost([FromBody] RequestDowntimeCostCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }

    [HttpPost("percentage-comparison")]
    public async Task<IActionResult> UpdatePercentageComparison([FromBody] RequestPercentageComparisonCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _activityReportCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = ActivityReportResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }

    [HttpGet("by-admin/{adminId:int}")]
    public async Task<IActionResult> GetActivityReportsByAdmin([FromRoute] int adminId, CancellationToken cancellationToken)
    {
        var authenticatedAdminId = ((User)HttpContext.Items["User"]!).Id;
        if (adminId != authenticatedAdminId) return Forbid();

        var reports = await _activityReportQueryService.Handle(
            new GetAllActivityReportsByAdminIdQuery(adminId), cancellationToken);
        return Ok(reports.Select(ActivityReportResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
