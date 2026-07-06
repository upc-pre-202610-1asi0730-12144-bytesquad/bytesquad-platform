namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record CreateBranchCommand(int GymId, int AdminId, string Name, string Street, string District, string City);
