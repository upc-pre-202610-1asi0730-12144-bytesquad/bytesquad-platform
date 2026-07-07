namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record TechnicianResource(int Id, string Name, string Specialization, string? PhoneNumber, int AdminId);
