namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record RegisterEquipmentCommand(string Name, string Model, int ZoneId);
