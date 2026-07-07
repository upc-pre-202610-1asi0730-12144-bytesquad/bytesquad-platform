using SpotTrack.Platform.Reservations.Domain.Model.Projections;
using SpotTrack.Platform.Reservations.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Reservations.Interfaces.Rest.Transform;

public static class HourlyUsageStatResourceAssembler
{
    public static HourlyUsageStatResource ToResourceFromProjection(HourlyUsageStat stat) =>
        new(stat.Hour, stat.ReservationCount, Math.Round(stat.TotalMinutes, 1));
}
