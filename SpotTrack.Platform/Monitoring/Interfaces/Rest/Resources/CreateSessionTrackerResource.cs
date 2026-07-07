namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record CreateSessionTrackerResource(int EquipmentId, DateTimeOffset StartedAt);
