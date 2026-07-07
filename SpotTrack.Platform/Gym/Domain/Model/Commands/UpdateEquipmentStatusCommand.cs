namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record UpdateEquipmentStatusCommand(int EquipmentId, string Status);
