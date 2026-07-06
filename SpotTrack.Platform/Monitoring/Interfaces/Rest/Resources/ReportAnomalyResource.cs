namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record ReportAnomalyResource(int ReservationId, int EquipmentId, int ZoneId, string AnomalyDescription);
