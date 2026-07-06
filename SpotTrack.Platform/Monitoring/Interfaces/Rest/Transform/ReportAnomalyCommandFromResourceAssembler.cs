using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;

public static class ReportAnomalyCommandFromResourceAssembler
{
    public static ReportAnomalyCommand ToCommandFromResource(ReportAnomalyResource resource) =>
        new(resource.ReservationId, resource.EquipmentId, resource.ZoneId, resource.AnomalyDescription);
}
