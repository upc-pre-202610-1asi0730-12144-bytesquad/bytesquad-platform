namespace SpotTrack.Platform.Alerts.Domain.Model.Commands;

public record ResolveAlertCommand(int AlertId, int AdminId);
