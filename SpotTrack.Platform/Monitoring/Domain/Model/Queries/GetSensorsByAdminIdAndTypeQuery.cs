namespace SpotTrack.Platform.Monitoring.Domain.Model.Queries;

public record GetSensorsByAdminIdAndTypeQuery(int AdminId, SensorType SensorType);
