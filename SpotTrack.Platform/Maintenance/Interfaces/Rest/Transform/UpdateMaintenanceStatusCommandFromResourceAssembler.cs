using SpotTrack.Platform.Maintenances.Domain.Model;
using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class UpdateMaintenanceStatusCommandFromResourceAssembler
{
    public static UpdateMaintenanceStatusCommand ToCommandFromResource(int ticketId, UpdateMaintenanceStatusResource resource) =>
        new(ticketId, Enum.Parse<EMaintenanceProgress>(resource.NewProgress, ignoreCase: true));
}
