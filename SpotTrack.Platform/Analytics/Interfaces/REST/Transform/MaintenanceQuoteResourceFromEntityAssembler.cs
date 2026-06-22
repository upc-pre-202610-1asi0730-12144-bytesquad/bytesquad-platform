using SpotTrack.Platform.Analytics.Domain.Model.Aggregates;
using SpotTrack.Platform.Analytics.Interfaces.REST.Resources;

namespace SpotTrack.Platform.Analytics.Interfaces.REST.Transform;

public static class MaintenanceQuoteResourceFromEntityAssembler
{
    public static MaintenanceQuoteResource ToResourceFromEntity(MaintenanceQuote entity)
    {
        return new MaintenanceQuoteResource(
            entity.Id,
            entity.MaintenanceQuoteId.Value,
            entity.CorrectiveActionsCost,
            entity.SparePartsCost,
            entity.PreventiveCost,
            entity.TotalMaintenanceCost
        );
    }
}