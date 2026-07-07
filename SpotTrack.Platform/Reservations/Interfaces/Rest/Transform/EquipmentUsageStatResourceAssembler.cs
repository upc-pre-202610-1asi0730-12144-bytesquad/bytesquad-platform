using SpotTrack.Platform.Reservations.Domain.Model.Projections;
using SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Transform;

public static class EquipmentUsageStatResourceAssembler
{
    public static EquipmentUsageStatResource ToResourceFromProjection(EquipmentUsageStat stat) =>
        new(stat.EquipmentId, Math.Round(stat.TotalUsageHours, 2), stat.ReservationCount);
}
