using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using SpotTrack.Platform.Maintenances.Application.CommandServices;
using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest;

[ApiController]
[Route("api/v1/maintenance/technicians")]
[Produces(MediaTypeNames.Application.Json)]
[Authorize(UserRole.Admin)]
[SwaggerTag("Maintenance technician management endpoints")]
public class TechnicianController(
    ITechnicianCommandService technicianCommandService,
    ITechnicianQueryService technicianQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a technician",
        Description = "Registers a new maintenance technician associated with the authenticated admin.",
        OperationId = "CreateTechnician")]
    [SwaggerResponse(StatusCodes.Status201Created, "Technician created successfully", typeof(TechnicianResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid technician data")]
    public async Task<IActionResult> CreateTechnician(
        [FromBody] CreateTechnicianResource resource,
        CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var command = CreateTechnicianCommandFromResourceAssembler.ToCommandFromResource(adminId, resource);
        var result = await technicianCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return TechnicianActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return TechnicianActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            TechnicianResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all technicians",
        Description = "Returns all technicians associated with the authenticated admin.",
        OperationId = "GetTechnicians")]
    [SwaggerResponse(StatusCodes.Status200OK, "Technicians retrieved successfully", typeof(IEnumerable<TechnicianResource>))]
    public async Task<IActionResult> GetTechnicians(CancellationToken cancellationToken)
    {
        var adminId = ((User)HttpContext.Items["User"]!).Id;
        var technicians = await technicianQueryService.Handle(
            new GetAllTechniciansByAdminIdQuery(adminId), cancellationToken);
        return Ok(technicians.Select(TechnicianResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
