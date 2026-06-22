namespace SpotTrack.Platform.Analytics.Domain.Model.Commands;

public record RequestPreventiveCostCommand(long MaintenanceQuoteId, double PreventiveCost);