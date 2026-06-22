using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Reservations.Domain.Model.Events;

public record ExpressReservationInitiatedEvent(
    int ReservationId,
    int ClientId,
    int EquipmentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate) : IEvent
{
    public static ExpressReservationInitiatedEvent FromReservation(Reservation reservation) =>
        new(reservation.Id,
            reservation.ClientId,
            reservation.EquipmentId,
            reservation.StartDate,
            reservation.EndDate);
}
