namespace SpotTrack.Platform.Analytics.Interfaces.REST.Resources;

public record MaintenanceQuoteResource(
    long Id,
    long MaintenanceQuoteId,
    double CorrectiveActionsCost,
    double SparePartsCost,
    double PreventiveCost,
    double TotalMaintenanceCost
);