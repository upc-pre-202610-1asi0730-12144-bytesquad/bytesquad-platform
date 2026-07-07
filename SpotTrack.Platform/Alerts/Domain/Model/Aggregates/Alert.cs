using SpotTrack.Platform.Alerts.Domain.Model.Commands;
using SpotTrack.Platform.Alerts.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Alerts.Domain.Model.Aggregates;

public class Alert
{
    public int Id { get; private set; }
    public int AdminId { get; private set; }
    public int? EquipmentId { get; private set; }
    public EAlertSeverity Severity { get; private set; }
    public string Message { get; private set; } = null!;
    public bool Resolved { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private Alert() { }

    public Alert(CreateAlertCommand command)
    {
        AdminId = command.AdminId;
        EquipmentId = command.EquipmentId;
        Severity = command.Severity;
        Message = command.Message;
        Resolved = false;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Resolve() => Resolved = true;
}
