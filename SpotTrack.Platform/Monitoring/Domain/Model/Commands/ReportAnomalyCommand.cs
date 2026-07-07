namespace SpotTrack.Platform.Monitoring.Domain.Model.Commands;

public record ReportAnomalyCommand(int SensorId, string AnomalyType, string Description, DateTimeOffset DetectedAt);
