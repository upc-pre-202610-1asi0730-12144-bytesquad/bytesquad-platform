using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Transform;

public static class ReservationResourceFromEntityAssembler
{
    public static CreateReservationResource ToResourceFromEntity(Reservation reservation) =>
        new(reservation.Id,
            reservation.ClientId,
            reservation.EquipmentId,
            reservation.StartDate,
            reservation.EndDate,
            reservation.Status.ToString()); // enum → string, no magic strings
}