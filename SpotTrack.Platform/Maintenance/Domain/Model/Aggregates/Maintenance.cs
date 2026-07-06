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

    /// <summary>Triggered when a TechnicalTicket is created for this maintenance request.</summary>
    public void Accept()
    {
        if (Status is not EMaintenanceStatus.Requested)
            throw new InvalidOperationException(
                $"Cannot accept a maintenance that is in '{Status}' status.");
        Status = EMaintenanceStatus.InProgress;
    }

    /// <summary>Triggered when a technician is assigned to the associated TechnicalTicket.</summary>
    public void MarkInProgress()
    {
        if (Status is not EMaintenanceStatus.Requested)
            throw new InvalidOperationException(
                $"Cannot mark as in-progress a maintenance that is in '{Status}' status.");
        Status = EMaintenanceStatus.InProgress;
    }

    public void Complete()
    {
        if (Status is not EMaintenanceStatus.InProgress)
            throw new InvalidOperationException(
                $"Cannot complete a maintenance that is in '{Status}' status.");
        Status = EMaintenanceStatus.Completed;
    }

    public void Cancel()
    {
        if (Status is EMaintenanceStatus.Completed or EMaintenanceStatus.Cancelled)
            throw new InvalidOperationException(
                $"Cannot cancel a maintenance that is already in '{Status}' status.");
        Status = EMaintenanceStatus.Cancelled;
    }
}
