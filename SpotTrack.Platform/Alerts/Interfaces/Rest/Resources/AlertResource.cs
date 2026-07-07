namespace SpotTrack.Platform.Alerts.Interfaces.Rest.Resources;

public record AlertResource(
    int Id,
    int AdminId,
    int? EquipmentId,
    string Severity,
    string Message,
    bool Resolved,
    DateTimeOffset CreatedAt);
