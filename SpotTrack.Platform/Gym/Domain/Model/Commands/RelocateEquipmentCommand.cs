namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record RelocateEquipmentCommand(int EquipmentId, int NewZoneId);
