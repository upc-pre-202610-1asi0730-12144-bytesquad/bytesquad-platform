using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotTrack.Platform.Reservations.Application.CommandServices;
using SpotTrack.Platform.Reservations.Domain.Model;
using SpotTrack.Platform.Reservations.Domain.Model.Commands;
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
}