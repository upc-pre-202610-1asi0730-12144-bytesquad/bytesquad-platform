namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

public record CreateReservationResource(
    int Id,
    int ClientId,
    int EquipmentId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Status);
