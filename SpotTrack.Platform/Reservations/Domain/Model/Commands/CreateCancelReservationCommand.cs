namespace SpotTrack.Platform.Reservations.Domain.Model.Commands;

public record CreateCancelReservationCommand(
    int ReservationId,
    int ClientId
);