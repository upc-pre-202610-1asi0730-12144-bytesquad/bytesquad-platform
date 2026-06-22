using SpotTrack.Platform.Reservations.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Reservations.Domain.Model.Events;

public record RequestOccupyEquipmentSubmittedEvent(
    int ReservationId,
    int ClientId,
    int EquipmentId
): IEvent
{
    public static RequestOccupyEquipmentSubmittedEvent FromReservation(Reservation reservation) =>
        new (reservation.Id, 
            reservation.ClientId, 
            reservation.EquipmentId);
    
}