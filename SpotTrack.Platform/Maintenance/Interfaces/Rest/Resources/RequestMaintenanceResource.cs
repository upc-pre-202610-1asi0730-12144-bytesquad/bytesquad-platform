using SpotTrack.Platform.Maintenances.Domain.Model;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record RequestMaintenanceResource(int EquipmentId, string Reason, EMaintenancePriority Priority, EMaintenanceType Type);
