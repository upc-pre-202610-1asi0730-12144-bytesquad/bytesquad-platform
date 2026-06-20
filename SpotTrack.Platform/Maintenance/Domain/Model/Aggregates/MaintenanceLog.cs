using SpotTrack.Platform.Maintenances.Domain.Model.Commands;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;

public partial class MaintenanceLog
{
    private MaintenanceLog() { }

    public MaintenanceLog(RegisterMaintenanceCompletionCommand command, int equipmentId)
    {
        TechnicalTicketId = command.TechnicalTicketId;
        EquipmentId = equipmentId;
        CompletedByAdminId = command.CompletedByAdminId;
        Notes = command.Notes;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; private set; }
    public int TechnicalTicketId { get; private set; }
    public int EquipmentId { get; private set; }
    public int CompletedByAdminId { get; private set; }
    public DateTimeOffset CompletedAt { get; private set; }
    public string Notes { get; private set; } = string.Empty;
}
