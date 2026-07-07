namespace SpotTrack.Platform.Monitoring.Domain.Model.Commands;

public record RegisterSensorCommand(int AdminId, SensorType SensorType, string Identifier, int? EquipmentId);
