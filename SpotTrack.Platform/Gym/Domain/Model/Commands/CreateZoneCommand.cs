namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record CreateZoneCommand(int GymId, int BranchId, string Name);
