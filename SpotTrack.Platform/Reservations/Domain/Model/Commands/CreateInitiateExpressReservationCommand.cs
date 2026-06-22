namespace SpotTrack.Platform.Reservations.Domain.Model.Commands;

public record CreateInitiateExpressReservationCommand(
    int ClientId,
    int EquipmentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate
);
