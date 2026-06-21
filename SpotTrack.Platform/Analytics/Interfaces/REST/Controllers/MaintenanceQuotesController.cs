using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Analytics.Application.CommandServices;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Interfaces.REST.Transform;

namespace SpotTrack.Platform.Analytics.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class MaintenanceQuotesController : ControllerBase
{
    private readonly IMaintenanceQuoteCommandService _maintenanceQuoteCommandService;

    public MaintenanceQuotesController(IMaintenanceQuoteCommandService maintenanceQuoteCommandService)
    {
        _maintenanceQuoteCommandService = maintenanceQuoteCommandService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaintenanceQuote([FromBody] RequestCorrectiveActionsCostCommand command)
    {
        var maintenanceQuote = await _maintenanceQuoteCommandService.Handle(command);
        if (maintenanceQuote == null) return BadRequest();

        var resource = MaintenanceQuoteResourceFromEntityAssembler.ToResourceFromEntity(maintenanceQuote);
        return StatusCode(201, resource);
    }
}