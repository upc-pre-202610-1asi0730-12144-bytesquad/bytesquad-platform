using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Interfaces.REST.Transform;

namespace SpotTrack.Platform.Analytics.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
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
        var roiProjection = await _roiProjectionCommandService.Handle(command);
        if (roiProjection == null) return BadRequest();

        var resource = ROIProjectionResourceFromEntityAssembler.ToResourceFromEntity(roiProjection);
        return StatusCode(201, resource);
    }
    
    [HttpPost("projected-earnings")]
    public async Task<IActionResult> UpdateProjectedEarnings([FromBody] RequestEarningsProjectionCommand command)
    {
        var roiProjection = await _roiProjectionCommandService.Handle(command);
        if (roiProjection == null) return NotFound();

        var resource = ROIProjectionResourceFromEntityAssembler.ToResourceFromEntity(roiProjection);
        return Ok(resource);
    }
    
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateROIProjection([FromBody] RequestROICommand command)
    {
        var roiProjection = await _roiProjectionCommandService.Handle(command);
        if (roiProjection == null) return NotFound();

        var resource = ROIProjectionResourceFromEntityAssembler.ToResourceFromEntity(roiProjection);
        return Ok(resource);
    }


}