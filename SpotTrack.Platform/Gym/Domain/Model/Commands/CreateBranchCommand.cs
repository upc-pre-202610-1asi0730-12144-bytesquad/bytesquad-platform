namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record CreateBranchCommand(int GymId, string Name, string Street, string District, string City);
