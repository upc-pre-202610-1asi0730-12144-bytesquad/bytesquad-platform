namespace SpotTrack.Platform.Monitoring.Domain.Model.Commands;

public record ReportAnomalyCommand(int ReservationId, int EquipmentId, int ZoneId, string AnomalyDescription);
