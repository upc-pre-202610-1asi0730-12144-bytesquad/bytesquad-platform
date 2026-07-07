namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record RequestMaintenanceResource(int EquipmentId, string Reason, string Priority, string Type);
