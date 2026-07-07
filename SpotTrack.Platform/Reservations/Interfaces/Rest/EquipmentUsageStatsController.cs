using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Reservations.Application.QueryServices;
using SpotTrack.Platform.Reservations.Domain.Model.Queries;
using SpotTrack.Platform.Reservations.Interfaces.Rest.Transform;

namespace SpotTrack.Platform.Reservations.Interfaces.Rest;

/// <summary>
/// Exposes aggregated equipment usage statistics derived from reservation data.
/// The route intentionally omits the "reservations" prefix: this is a dashboard
/// analytics surface, not a reservation CRUD resource. The controller lives in
/// the Reservations BC because that BC owns the source data.
/// </summary>
[ApiController]
[Route("api/v1/equipment-usage-stats")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
public class EquipmentUsageStatsController(IEquipmentUsageStatsQueryService queryService) : ControllerBase
{
    [HttpGet("by-admin/{adminId:int}")]
    public async Task<IActionResult> GetEquipmentUsageStats(
        [FromRoute] int adminId,
        CancellationToken cancellationToken)
    {
        var authenticatedAdminId = ((User)HttpContext.Items["User"]!).Id;
        if (adminId != authenticatedAdminId) return Forbid();

        var stats = await queryService.Handle(
            new GetEquipmentUsageStatsByAdminIdQuery(adminId), cancellationToken);
        return Ok(stats.Select(EquipmentUsageStatResourceAssembler.ToResourceFromProjection));
    }

    [HttpGet("peak-capacity/by-admin/{adminId:int}")]
    public async Task<IActionResult> GetPeakCapacityHours(
        [FromRoute] int adminId,
        CancellationToken cancellationToken)
    {
        var authenticatedAdminId = ((User)HttpContext.Items["User"]!).Id;
        if (adminId != authenticatedAdminId) return Forbid();

        var stats = await queryService.Handle(
            new GetPeakCapacityHoursByAdminIdQuery(adminId), cancellationToken);
        return Ok(stats.Select(HourlyUsageStatResourceAssembler.ToResourceFromProjection));
    }
}
