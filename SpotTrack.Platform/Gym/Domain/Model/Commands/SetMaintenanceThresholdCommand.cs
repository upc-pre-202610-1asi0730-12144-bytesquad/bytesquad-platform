namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record SetMaintenanceThresholdCommand(int EquipmentId, int ThresholdUsageCount);
