namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record AnomalyResource(
    int Id,
    int ReservationId,
    int EquipmentId,
    int ZoneId,
    string AnomalyDescription,
    DateTimeOffset EmissionDate);
