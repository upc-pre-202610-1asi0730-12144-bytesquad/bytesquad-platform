namespace SpotTrack.Platform.Gym.Domain.Model.Commands;

public record CreateGymCommand(string Name, string Street, string District, string City);
