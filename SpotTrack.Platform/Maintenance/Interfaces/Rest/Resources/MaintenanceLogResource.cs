namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record MaintenanceLogResource(
    int Id,
    int TechnicalTicketId,
    int EquipmentId,
    int CompletedByAdminId,
    DateTimeOffset CompletedAt,
    string Notes);
