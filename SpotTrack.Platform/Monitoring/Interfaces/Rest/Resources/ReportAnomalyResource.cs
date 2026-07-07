namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record ReportAnomalyResource(int SensorId, string AnomalyType, string Description, DateTimeOffset DetectedAt);
