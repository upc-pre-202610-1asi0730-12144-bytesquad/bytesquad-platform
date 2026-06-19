using SpotTrack.Platform.Maintenances.Domain.Model.Commands;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;

public partial class TechnicalTicket
{
    private TechnicalTicket() { }

    public TechnicalTicket(CreateTechnicalTicketCommand command)
    {
        if (command.MaintenanceId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.MaintenanceId), command.MaintenanceId,
                "MaintenanceId must be a positive integer.");

        if (command.EquipmentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.EquipmentId), command.EquipmentId,
                "EquipmentId must be a positive integer.");

        MaintenanceId = command.MaintenanceId;
        EquipmentId = command.EquipmentId;
        Description = command.Description;
        Status = ETechnicalTicketStatus.Created;
        MaintenanceProgress = EMaintenanceProgress.Pending;
        AssignedTechnicianId = null;
    }

    public int Id { get; private set; }
    public int MaintenanceId { get; private set; }
    public int EquipmentId { get; private set; }
    public ETechnicalTicketStatus Status { get; private set; }
    public EMaintenanceProgress MaintenanceProgress { get; private set; }
    public int? AssignedTechnicianId { get; private set; }
    public string Description { get; private set; } = string.Empty;

    public void Assign(int technicianId)
    {
        if (Status is not ETechnicalTicketStatus.Created)
            throw new InvalidOperationException(
                $"Cannot assign a technical ticket that is in '{Status}' status.");

        AssignedTechnicianId = technicianId;
        Status = ETechnicalTicketStatus.Assigned;
    }

    // Lifecycle methods for future features (ModifyStatus, RequestUpdateMaintenanceStatus,
    // UpdateMaintenanceStatus, Complete) will be added here.
}
