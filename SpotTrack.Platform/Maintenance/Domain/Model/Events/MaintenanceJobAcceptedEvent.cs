using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Events;

public record MaintenanceJobAcceptedEvent(
    int MaintenanceJobId,
    int TechnicalTicketId,
    int TechnicianId) : IEvent
{
    public static MaintenanceJobAcceptedEvent FromMaintenanceJob(MaintenanceJob job) =>
        new(job.Id,
            job.TechnicalTicketId,
            job.TechnicianId);
}
