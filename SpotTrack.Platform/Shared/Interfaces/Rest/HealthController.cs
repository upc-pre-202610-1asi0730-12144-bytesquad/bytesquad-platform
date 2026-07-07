using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Shared.Interfaces.Rest;

[ApiController]
[Route("api/v1/health")]
[Produces(MediaTypeNames.Application.Json)]
[AllowAnonymous]
[SwaggerTag("Health check endpoint")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Health check", OperationId = "HealthCheck")]
    [SwaggerResponse(StatusCodes.Status200OK, "Service is healthy")]
    public IActionResult Get() => Ok(new { status = "healthy" });
}
