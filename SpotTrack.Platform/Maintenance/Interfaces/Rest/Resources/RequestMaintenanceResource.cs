namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record RequestMaintenanceResource(int EquipmentId, int RequestedByAdminId, string Reason);
