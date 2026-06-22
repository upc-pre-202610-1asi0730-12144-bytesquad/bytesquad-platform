using SpotTrack.Platform.Maintenances.Domain.Model.Commands;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;

public partial class Maintenance
{
    private Maintenance() { }

    public Maintenance(CreateRequestMaintenanceCommand command)
    {
        if (command.EquipmentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.EquipmentId), command.EquipmentId,
                "EquipmentId must be a positive integer.");

        if (command.RequestedByAdminId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.RequestedByAdminId), command.RequestedByAdminId,
                "RequestedByAdminId must be a positive integer.");

        EquipmentId = command.EquipmentId;
        RequestedByAdminId = command.RequestedByAdminId;
        Reason = command.Reason;
        Status = EMaintenanceStatus.Requested;
    }

    public int Id { get; private set; }
    public int EquipmentId { get; private set; }
    public int RequestedByAdminId { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public EMaintenanceStatus Status { get; private set; }
}
