namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record CreateTechnicalTicketResource(int MaintenanceId, int EquipmentId, string Description);
