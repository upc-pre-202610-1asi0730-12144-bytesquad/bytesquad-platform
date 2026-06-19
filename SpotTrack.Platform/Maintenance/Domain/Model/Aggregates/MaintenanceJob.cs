using SpotTrack.Platform.Maintenances.Domain.Model.Commands;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;

public partial class MaintenanceJob
{
    private MaintenanceJob() { }

    public MaintenanceJob(CreateAcceptMaintenanceCommand command)
    {
        if (command.TechnicalTicketId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.TechnicalTicketId), command.TechnicalTicketId,
                "TechnicalTicketId must be a positive integer.");

        if (command.TechnicianId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.TechnicianId), command.TechnicianId,
                "TechnicianId must be a positive integer.");

        TechnicalTicketId = command.TechnicalTicketId;
        TechnicianId = command.TechnicianId;
        Status = EMaintenanceJobStatus.Accepted;
    }

    public int Id { get; private set; }
    public int TechnicalTicketId { get; private set; }
    public int TechnicianId { get; private set; }
    public EMaintenanceJobStatus Status { get; private set; }
}
