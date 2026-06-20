namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record RegisterMaintenanceCompletionCommand(int TechnicalTicketId, int CompletedByAdminId, string Notes);
