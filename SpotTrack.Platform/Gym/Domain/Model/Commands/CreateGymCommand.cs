namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record CreateGymCommand(string Name, string Street, string District, string City);
