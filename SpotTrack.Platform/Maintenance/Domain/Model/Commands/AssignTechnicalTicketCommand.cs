namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record AssignTechnicalTicketCommand(int TechnicalTicketId, int TechnicianId);
