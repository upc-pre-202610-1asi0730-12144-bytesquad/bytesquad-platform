namespace SpotTrack.Platform.Analytics.Domain.Model.Commands;

public record RequestDowntimeCostCommand(long ActivityReportId, long DowntimeCost);
