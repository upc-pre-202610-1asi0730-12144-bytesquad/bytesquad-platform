namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record CreateSessionTrackerResource(int EquipmentId, int AdminId, DateTimeOffset StartedAt);
