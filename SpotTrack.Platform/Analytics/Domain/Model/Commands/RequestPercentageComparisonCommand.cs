namespace SpotTrack.Platform.Analytics.Domain.Model.Commands;

public record RequestPercentageComparisonCommand(long ActivityReportId, double PercentageComparison);