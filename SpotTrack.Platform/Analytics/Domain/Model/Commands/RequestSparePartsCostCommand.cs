namespace SpotTrack.Platform.Analytics.Domain.Model.Commands;

public record RequestSparePartsCostCommand(long MaintenanceQuoteId, double SparePartsCost);