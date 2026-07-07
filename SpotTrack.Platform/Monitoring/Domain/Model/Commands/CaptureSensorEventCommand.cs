namespace SpotTrack.Platform.Monitoring.Domain.Model.Commands;

public record CaptureSensorEventCommand(int SensorId, DateTimeOffset DetectedAt);
