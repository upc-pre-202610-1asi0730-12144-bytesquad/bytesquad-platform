using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class MaintenanceJobResourceFromEntityAssembler
{
    public static MaintenanceJobResource ToResourceFromEntity(MaintenanceJob job) =>
        new(job.Id,
            job.TechnicalTicketId,
            job.TechnicianId,
            job.Status.ToString());
}
