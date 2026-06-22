namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record CreateTechnicalTicketCommand(int MaintenanceId, int EquipmentId, string Description);
