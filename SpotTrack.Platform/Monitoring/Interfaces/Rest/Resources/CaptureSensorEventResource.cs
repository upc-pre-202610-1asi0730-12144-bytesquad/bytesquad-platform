namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record CaptureSensorEventResource(int SensorId, DateTimeOffset DetectedAt);
