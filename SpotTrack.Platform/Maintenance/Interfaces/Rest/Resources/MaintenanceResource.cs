namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record MaintenanceResource(int Id, int EquipmentId, int RequestedByAdminId, string Reason, string Status);
