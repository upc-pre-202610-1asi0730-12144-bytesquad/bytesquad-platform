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

    [HttpPost("{id:int}/request-status-update")]
    [SwaggerOperation(
        Summary = "Request a maintenance status update",
        Description = "Marks the maintenance progress of the ticket as InProgress, signalling that a status update has been requested.",
        OperationId = "RequestStatusUpdate")]
    [SwaggerResponse(StatusCodes.Status200OK, "Maintenance status update requested successfully",
        typeof(TechnicalTicketResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Technical ticket not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Ticket is not in a valid status to request a status update")]
    public async Task<IActionResult> RequestStatusUpdate(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new RequestUpdateMaintenanceStatusCommand(id);
        var result = await technicalTicketCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return TechnicalTicketActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return TechnicalTicketActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            TechnicalTicketResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpPut("{id:int}/status")]
    [SwaggerOperation(
        Summary = "Modify a technical ticket status",
        Description = "Transitions an existing technical ticket to a new status. Cannot revert to Created or modify a Resolved ticket.",
        OperationId = "ModifyTicketStatus")]
    [SwaggerResponse(StatusCodes.Status200OK, "Technical ticket status modified successfully",
        typeof(TechnicalTicketResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Technical ticket not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid status transition")]
    public async Task<IActionResult> ModifyTicketStatus(
        [FromRoute] int id,
        [FromBody] ModifyTicketStatusResource resource,
        CancellationToken cancellationToken)
    {
        var command = ModifyTicketStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await technicalTicketCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return TechnicalTicketActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return TechnicalTicketActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            TechnicalTicketResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpPut("{id:int}/maintenance-status")]
    [SwaggerOperation(
        Summary = "Update maintenance progress of a technical ticket",
        Description = "Sets the maintenance progress to the specified value (Pending, InProgress, or Completed). Not allowed on resolved tickets.",
        OperationId = "UpdateMaintenanceStatus")]
    [SwaggerResponse(StatusCodes.Status200OK, "Maintenance status updated successfully",
        typeof(TechnicalTicketResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Technical ticket not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Ticket is resolved and cannot be updated")]
    public async Task<IActionResult> UpdateMaintenanceStatus(
        [FromRoute] int id,
        [FromBody] UpdateMaintenanceStatusResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateMaintenanceStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
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
