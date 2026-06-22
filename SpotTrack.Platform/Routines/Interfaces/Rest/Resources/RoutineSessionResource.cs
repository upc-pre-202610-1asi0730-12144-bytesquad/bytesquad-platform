namespace SpotTrack.Platform.Routines.Interfaces.Rest.Resources;

public record RoutineSessionResource(int Id, int RoutineId, int ClientId, string Status, DateTimeOffset StartedAt);
