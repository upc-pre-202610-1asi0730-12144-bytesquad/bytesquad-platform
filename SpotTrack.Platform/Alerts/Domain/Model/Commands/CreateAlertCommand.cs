using SpotTrack.Platform.Alerts.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Alerts.Domain.Model.Commands;

public record CreateAlertCommand(int AdminId, int? EquipmentId, EAlertSeverity Severity, string Message);
