namespace SpotTrack.Platform.Analytics.Domain.Model.Commands;

public record RequestTotalUsageTimeCommand(long ActivityReportId, long TotalUsageTime);