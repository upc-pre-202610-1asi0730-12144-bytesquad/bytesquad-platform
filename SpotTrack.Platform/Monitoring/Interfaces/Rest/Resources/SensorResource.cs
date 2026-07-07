namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record SensorResource(int Id, string Type, string Identifier, int AdminId, int? EquipmentId, DateTimeOffset RegisteredAt, int CaptureCount);
