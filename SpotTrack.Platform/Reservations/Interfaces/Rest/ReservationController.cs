using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Reservations.Application.CommandServices;
using SpotTrack.Platform.Reservations.Application.QueryServices;
using SpotTrack.Platform.Reservations.Domain.Model;
using SpotTrack.Platform.Reservations.Domain.Model.Commands;
using SpotTrack.Platform.Reservations.Domain.Model.Queries;
using SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;
using SpotTrack.Platform.Reservations.Interfaces.Rest.Transform;
using SpotTrack.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace SpotTrack.Platform.Reservations.Interfaces.Rest;

[ApiController]
[Route("api/v1/reservations")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Reservation management endpoints")]
public class ReservationsController(
    IReservationCommandService reservationCommandService,
    IReservationQueryService reservationQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    [HttpPost("express")]
    [SwaggerOperation(
        Summary = "Initiate an express reservation",
        Description = "Creates a new express reservation for a client on a given equipment and period.",
        OperationId = "InitiateExpressReservation")]
    [SwaggerResponse(StatusCodes.Status201Created, "Reservation initiated successfully",
        typeof(CreateReservationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid reservation data provided")]
    public async Task<IActionResult> InitiateExpressReservation(
        [FromBody] CreateInitiateExpressReservationResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateInitiateExpressReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reservationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return ReservationsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return ReservationsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ReservationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status201Created,
            this);
    }

    [HttpDelete("{id}/cancel")]
    [SwaggerOperation(
        Summary = "Cancel an express reservation",
        Description = "Cancel a existing express reservation for a client",
        OperationId = "CancelExpressReservation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reservation cancelled successfully", typeof(CreateReservationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reservation not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Reservation cannot be cancelled")]
    public async Task<IActionResult> CancelReservation(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new CreateCancelReservationCommand(id);
        var result = await reservationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return ReservationsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return ReservationsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ReservationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpPost("{id}/submit-request")]
    [SwaggerOperation(
        Summary = "Submit a request reservation",
        Description = "Submit a request reservation for a client",
        OperationId = "SubmitReservation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reservation submitted successfully", typeof(CreateReservationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reservation not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Reservation cannot be submitted")]
    public async Task<IActionResult> SubmitReservation(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new CreateSubmitRequestOccupyEquipmentCommand(id);
        var result = await reservationCommandService.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return ReservationsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return ReservationsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ReservationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }
    
    [HttpPost("{id}/end")]
    [SwaggerOperation(
        Summary = "End a reservation",
        Description = "End a reservation for a client",
        OperationId = "EndReservation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reservation cancelled successfully", typeof(CreateReservationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reservation not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Reservation cannot be cancelled")]
    public async Task<IActionResult> EndReservation(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new CreateEndReservationCommand(id);
        var result = await reservationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return ReservationsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return ReservationsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ReservationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpPost("{id}/request-equipment-available")]
    [SwaggerOperation(
        Summary = "Request equipment status change to available",
        Description = "Signals that the equipment should be released back to available in the Gym context.",
        OperationId = "RequestEquipmentAvailable")]
    [SwaggerResponse(StatusCodes.Status200OK, "Equipment release requested successfully", typeof(CreateReservationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reservation not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Reservation status does not allow this operation or equipment release failed")]
    public async Task<IActionResult> RequestEquipmentAvailable(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new CreateRequestEquipmentStatusChangeToAvailableCommand(id);
        var result = await reservationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return ReservationsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return ReservationsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ReservationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Get a reservation by ID",
        Description = "Returns the reservation matching the given ID, or 404 if not found.",
        OperationId = "GetReservationById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reservation found", typeof(CreateReservationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reservation not found")]
    public async Task<IActionResult> GetReservationById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var reservation = await reservationQueryService.Handle(new GetReservationByIdQuery(id), cancellationToken);
        if (reservation is null)
            return problemDetailsFactory.CreateProblemDetails(
                this, StatusCodes.Status404NotFound, ReservationsError.ReservationNotFound, "Reservation not found.");
        return Ok(ReservationResourceFromEntityAssembler.ToResourceFromEntity(reservation));
    }

    [HttpGet("by-client/{clientId:int}")]
    [SwaggerOperation(
        Summary = "Get all reservations by client ID",
        Description = "Returns all reservations for the given client.",
        OperationId = "GetReservationsByClientId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of reservations for the client", typeof(IEnumerable<CreateReservationResource>))]
    public async Task<IActionResult> GetReservationsByClientId(
        [FromRoute] int clientId,
        CancellationToken cancellationToken)
    {
        var reservations = await reservationQueryService.Handle(
            new GetAllReservationsByClientIdQuery(clientId), cancellationToken);
        var resources = reservations.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("by-equipment/{equipmentId:int}")]
    [SwaggerOperation(
        Summary = "Get all reservations by equipment ID",
        Description = "Returns all reservations for the given equipment.",
        OperationId = "GetReservationsByEquipmentId")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of reservations for the equipment", typeof(IEnumerable<CreateReservationResource>))]
    public async Task<IActionResult> GetReservationsByEquipmentId(
        [FromRoute] int equipmentId,
        CancellationToken cancellationToken)
    {
        var reservations = await reservationQueryService.Handle(
            new GetAllReservationsByEquipmentIdQuery(equipmentId), cancellationToken);
        var resources = reservations.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost("{id}/start-timer")]
    [SwaggerOperation(
        Summary = "Start the reservation timer",
        Description = "Transitions a Reserved reservation to Active and marks the equipment as occupied.",
        OperationId = "StartReservationTimer")]
    [SwaggerResponse(StatusCodes.Status200OK, "Reservation timer started successfully", typeof(CreateReservationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Reservation not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Reservation cannot be started or equipment occupy failed")]
    public async Task<IActionResult> StartReservationTimer(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new CreateStartReservationTimerCommand(id);
        var result = await reservationCommandService.Handle(command, cancellationToken);

        if (result.IsFailure)
            return ReservationsActionResultAssembler.ToFailureActionResult(result, this, problemDetailsFactory);

        return ReservationsActionResultAssembler.ToSuccessActionResult(
            result.Value!,
            ReservationResourceFromEntityAssembler.ToResourceFromEntity,
            StatusCodes.Status200OK,
            this);
    }
}