using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model;
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
public class ROIProjectionsController : ControllerBase
{
    private readonly IROIProjectionCommandService _roiProjectionCommandService;

    public ROIProjectionsController(IROIProjectionCommandService roiProjectionCommandService)
    {
        _roiProjectionCommandService = roiProjectionCommandService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateROIProjection([FromBody] RequestDowntimeCostProjectionCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _roiProjectionCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure) return BadRequest(result.Message);

        var resource = ROIProjectionResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return StatusCode(201, resource);
    }

    [HttpPost("projected-earnings")]
    public async Task<IActionResult> UpdateProjectedEarnings([FromBody] RequestEarningsProjectionCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _roiProjectionCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = ROIProjectionResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateROIProjection([FromBody] RequestROICommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _roiProjectionCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = ROIProjectionResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }
}
