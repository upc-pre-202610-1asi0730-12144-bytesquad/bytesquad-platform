namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record AnomalyResource(int Id, int SensorId, int AdminId, string AnomalyType, string Description, DateTimeOffset DetectedAt);
