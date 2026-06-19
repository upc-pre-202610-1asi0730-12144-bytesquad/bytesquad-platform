namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record MaintenanceJobResource(int Id, int TechnicalTicketId, int TechnicianId, string Status);
