using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Reservations.Application.CommandServices;
using SpotTrack.Platform.Reservations.Domain.Model;
using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Domain.Model.Commands;
using SpotTrack.Platform.Reservations.Domain.Model.Events;
using SpotTrack.Platform.Reservations.Domain.Repositories;
using SpotTrack.Platform.Reservations.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Reservations.Application.Internal.CommandServices;

public class ReservationCommandService(
    IReservationRepository reservationRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<ReservationMessages> localizer)
    : IReservationCommandService
{
    public async Task<Result<Reservation>> Handle(
        CreateInitiateExpressReservationCommand command,
        CancellationToken cancellationToken)
    {
        Reservation reservation;

        try
        {
            reservation = new Reservation(command);
        }
        catch (ArgumentOutOfRangeException)
        {
            return Result<Reservation>.Failure(
                ReservationsError.InvalidReservationDates,
                localizer[nameof(ReservationsError.InvalidReservationDates)]);
        }

        try
        {
            await reservationRepository.AddAsync(reservation, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            await mediator.PublishAsync(
                ExpressReservationInitiatedEvent.FromReservation(reservation),
                cancellationToken);

            return Result<Reservation>.Success(reservation);
        }
        catch (OperationCanceledException)
        {
            return Result<Reservation>.Failure(
                ReservationsError.OperationCancelled,
                localizer[nameof(ReservationsError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Reservation>.Failure(
                ReservationsError.DatabaseError,
                localizer[nameof(ReservationsError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Reservation>.Failure(
                ReservationsError.InternalServerError,
                localizer[nameof(ReservationsError.InternalServerError)]);
        }
    }

    public async Task<Result<Reservation>> Handle(
        CreateCancelReservationCommand command,
        CancellationToken cancellationToken)
    {

        var reservation = await reservationRepository.FindByIdAsync(command.ReservationId,
            cancellationToken);
        if (reservation is null)
            return Result<Reservation>.Failure(
                ReservationsError.ReservationNotFound,
                localizer[nameof(ReservationsError.ReservationNotFound)]);
        try
        {
            reservation.Cancel();
        }
        catch (InvalidOperationException ex)
        {
            return Result<Reservation>.Failure(
                ReservationsError.InvalidReservationStatus,
                ex.Message);
        }
        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Reservation>.Success(reservation);
        }
        catch (DbUpdateException)
        {
            return Result<Reservation>.Failure(
                ReservationsError.DatabaseError,
                localizer[nameof(ReservationsError.DatabaseError)]);
        }
    }

    public async Task<Result<Reservation>> Handle(
        CreateSubmitRequestOccupyEquipmentCommand command,
        CancellationToken cancellationToken)
    {
        var reservation = await reservationRepository.FindByIdAsync(command.ReservationId, cancellationToken);
        if (reservation is null)
        {
            return Result<Reservation>.Failure(
                ReservationsError.ReservationNotFound,
                localizer [nameof(ReservationsError.ReservationNotFound)]);
        }

        try
        {
            reservation.SubmitRequest();
        }

        catch(InvalidOperationException ex)
        {
            return Result<Reservation>.Failure(
                ReservationsError.InvalidReservationStatus, ex.Message);
        }

        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Reservation>.Success(reservation);
        }
        catch (DbUpdateException)
        {
            return Result<Reservation>.Failure(
                ReservationsError.DatabaseError,
                localizer[nameof(ReservationsError.DatabaseError)]);
        }

        await mediator.PublishAsync(RequestOccupyEquipmentSubmittedEvent.FromReservation(reservation),
            cancellationToken);
        return Result<Reservation>.Success(reservation);
    }

    public async Task<Result<Reservation>> Handle(
        CreateEndReservationCommand command, CancellationToken cancellationToken)
    {
       var reservation = await reservationRepository.FindByIdAsync(command.ReservationId,
           cancellationToken);
       if (reservation is null)
           return Result<Reservation>.Failure(
               ReservationsError.ReservationNotFound,
               localizer[nameof(ReservationsError.ReservationNotFound)]);
        try
        {
            reservation.End();
        }
        catch (InvalidOperationException ex)
        {
            return Result<Reservation>.Failure(
                ReservationsError.InvalidReservationStatus,
                ex.Message);
        }
        try
        {
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Reservation>.Success(reservation);
        }
        catch (DbUpdateException)
        {
            return Result<Reservation>.Failure(
                ReservationsError.DatabaseError,
                localizer[nameof(ReservationsError.DatabaseError)]);
        }
    }
}




