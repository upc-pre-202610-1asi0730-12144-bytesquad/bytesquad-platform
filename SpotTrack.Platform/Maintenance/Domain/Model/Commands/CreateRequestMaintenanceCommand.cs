namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record CreateRequestMaintenanceCommand(int EquipmentId, int RequestedByAdminId, string Reason);
