namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record CreateTechnicianResource(string Name, string Specialization, string? PhoneNumber);
