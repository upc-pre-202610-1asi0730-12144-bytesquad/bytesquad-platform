namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record SessionTrackerTimeResource(int Id, bool IsActive, double ElapsedSeconds);
