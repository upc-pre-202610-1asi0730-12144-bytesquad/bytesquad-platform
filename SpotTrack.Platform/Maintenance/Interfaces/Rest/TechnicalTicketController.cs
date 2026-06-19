using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Maintenances.Application.CommandServices;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest;

[ApiController]
[Route("api/v1/technical-tickets")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Technical ticket management endpoints")]
public class TechnicalTicketController(
    ITechnicalTicketCommandService technicalTicketCommandService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a technical ticket",
        Description = "Creates a new technical ticket for a maintenance request, marking the equipment out of service.",
        OperationId = "CreateTechnicalTicket")]
    [SwaggerResponse(StatusCodes.Status201Created, "Technical ticket created successfully",
        typeof(TechnicalTicketResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Maintenance record not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Equipment update failed")]
    public async Task<IActionResult> CreateTechnicalTicket(
        [FromBody] CreateTechnicalTicketResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateTechnicalTicketCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await technicalTicketCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return TechnicalTicketActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return TechnicalTicketActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            TechnicalTicketResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpPost("{id:int}/assign")]
    [SwaggerOperation(
        Summary = "Assign a technical ticket to a technician",
        Description = "Assigns an existing technical ticket to a technician, transitioning its status to Assigned.",
        OperationId = "AssignTechnicalTicket")]
    [SwaggerResponse(StatusCodes.Status200OK, "Technical ticket assigned successfully",
        typeof(TechnicalTicketResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Technical ticket not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Technical ticket cannot be assigned in its current status")]
    public async Task<IActionResult> AssignTechnicalTicket(
        [FromRoute] int id,
        [FromBody] AssignTechnicalTicketResource resource,
        CancellationToken cancellationToken)
    {
        var command = AssignTechnicalTicketCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await technicalTicketCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return TechnicalTicketActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return TechnicalTicketActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            TechnicalTicketResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }
}
