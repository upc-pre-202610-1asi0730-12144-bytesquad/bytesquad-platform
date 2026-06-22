namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record TechnicalTicketResource(
    int Id,
    int MaintenanceId,
    int EquipmentId,
    string Status,
    string MaintenanceProgress,
    int? AssignedTechnicianId,
    string Description);
