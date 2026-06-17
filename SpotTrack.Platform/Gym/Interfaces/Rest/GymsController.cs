using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest;

[ApiController]
[Route("api/v1/gyms")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize]
[SwaggerTag("Gym management endpoints")]
public class GymsController(
    IGymCommandService gymCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new gym",
        Description = "Creates a new gym with the given name and address. Requires Admin authentication.",
        OperationId = "CreateGym")]
    [SwaggerResponse(StatusCodes.Status201Created, "Gym created successfully", typeof(GymResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid gym data provided")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
    public async Task<IActionResult> CreateGym(
        [FromBody] CreateGymResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateGymCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await gymCommandService.Handle(command, cancellationToken);
        if (result.IsFailure)
            return GymsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);
        return GymsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            GymResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }
}
