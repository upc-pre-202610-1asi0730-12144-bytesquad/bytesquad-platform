namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record CreateGymCommand(int AdminId, string Name, string Street, string District, string City);
