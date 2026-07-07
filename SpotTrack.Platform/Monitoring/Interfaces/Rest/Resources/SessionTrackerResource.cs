namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record SessionTrackerResource(int Id, int EquipmentId, int AdminId, DateTimeOffset StartedAt, DateTimeOffset? EndedAt, bool IsActive);
