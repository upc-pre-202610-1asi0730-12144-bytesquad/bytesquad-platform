namespace SpotTrack.Platform.Monitoring.Domain.Model.Commands;

public record CreateSessionTrackerCommand(int EquipmentId, int AdminId, DateTimeOffset StartedAt);
