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
public class MaintenanceQuotesController : ControllerBase
{
    private readonly IMaintenanceQuoteCommandService _maintenanceQuoteCommandService;
    private readonly IMaintenanceQuoteQueryService _maintenanceQuoteQueryService;

    public MaintenanceQuotesController(
        IMaintenanceQuoteCommandService maintenanceQuoteCommandService,
        IMaintenanceQuoteQueryService maintenanceQuoteQueryService)
    {
        _maintenanceQuoteCommandService = maintenanceQuoteCommandService;
        _maintenanceQuoteQueryService = maintenanceQuoteQueryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceQuote([FromBody] RequestCorrectiveActionsCostCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _maintenanceQuoteCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure) return BadRequest(result.Message);

        var resource = MaintenanceQuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return StatusCode(201, resource);
    }

    [HttpPost("spare-parts-cost")]
    public async Task<IActionResult> UpdateSparePartsCost([FromBody] RequestSparePartsCostCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _maintenanceQuoteCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = MaintenanceQuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }

    [HttpPost("preventive-cost")]
    public async Task<IActionResult> UpdatePreventiveCost([FromBody] RequestPreventiveCostCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _maintenanceQuoteCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = MaintenanceQuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }

    [HttpPost("total-cost")]
    public async Task<IActionResult> ConsolidateMaintenanceCost([FromBody] RequestMaintenanceCostCommand command)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var result = await _maintenanceQuoteCommandService.Handle(command with { AuthenticatedAdminId = adminId });
        if (result.IsFailure)
            return result.Error is AnalyticsError.Forbidden ? Forbid() : NotFound();

        var resource = MaintenanceQuoteResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
        return Ok(resource);
    }

    [HttpGet("by-admin/{adminId:int}")]
    public async Task<IActionResult> GetMaintenanceQuotesByAdmin([FromRoute] int adminId, CancellationToken cancellationToken)
    {
        var authenticatedAdminId = ((User)HttpContext.Items["User"]!).Id;
        if (adminId != authenticatedAdminId) return Forbid();

        var quotes = await _maintenanceQuoteQueryService.Handle(
            new GetAllMaintenanceQuotesByAdminIdQuery(adminId), cancellationToken);
        return Ok(quotes.Select(MaintenanceQuoteResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
