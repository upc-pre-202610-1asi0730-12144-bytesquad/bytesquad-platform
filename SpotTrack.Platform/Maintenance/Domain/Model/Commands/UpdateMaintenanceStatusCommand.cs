namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record UpdateMaintenanceStatusCommand(int TechnicalTicketId, EMaintenanceProgress NewProgress);
