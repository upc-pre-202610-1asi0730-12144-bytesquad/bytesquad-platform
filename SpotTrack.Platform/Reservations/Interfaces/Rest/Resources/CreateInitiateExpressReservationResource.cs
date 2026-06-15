namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

public record CreateInitiateExpressReservationResource(
    int ClientId,
    int EquipmentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);
