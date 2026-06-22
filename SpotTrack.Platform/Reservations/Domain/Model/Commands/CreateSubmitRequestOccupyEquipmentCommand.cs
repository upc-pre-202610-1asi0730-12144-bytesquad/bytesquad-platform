namespace SpotTrack.Platform.Reservations.Domain.Model.Commands;

public record CreateSubmitRequestOccupyEquipmentCommand (
    int ReservationId
    );