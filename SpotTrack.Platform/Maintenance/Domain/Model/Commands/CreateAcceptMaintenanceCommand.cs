namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record CreateAcceptMaintenanceCommand(int TechnicalTicketId, int TechnicianId);
