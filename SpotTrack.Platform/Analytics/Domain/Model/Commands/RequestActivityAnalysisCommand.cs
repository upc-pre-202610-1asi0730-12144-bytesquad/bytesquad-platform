namespace SpotTrack.Platform.Analytics.Domain.Model.Commands;

public record RequestActivityAnalysisCommand(long TotalUsageTime, long DowntimeCost, double PercentageComparison);