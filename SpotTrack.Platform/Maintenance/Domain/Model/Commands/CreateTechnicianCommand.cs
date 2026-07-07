namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record CreateTechnicianCommand(string Name, string Specialization, string? PhoneNumber, int AdminId);
