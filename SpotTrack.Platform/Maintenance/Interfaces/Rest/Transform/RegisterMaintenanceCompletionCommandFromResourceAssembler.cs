using SpotTrack.Platform.Maintenances.Domain.Model.Commands;
using SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Transform;

public static class RegisterMaintenanceCompletionCommandFromResourceAssembler
{
    public static RegisterMaintenanceCompletionCommand ToCommandFromResource(RegisterMaintenanceCompletionResource resource) =>
        new(resource.TechnicalTicketId, resource.CompletedByAdminId, resource.Notes);
}
