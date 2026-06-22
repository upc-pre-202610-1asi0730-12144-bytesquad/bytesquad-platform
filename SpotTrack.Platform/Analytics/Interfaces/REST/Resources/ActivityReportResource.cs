namespace SpotTrack.Platform.Analytics.Interfaces.REST.Resources;

public record ActivityReportResource(
    long Id, 
    long ActivityReportId, 
    long TotalUsageTime, 
    long DowntimeCost, 
    double PercentageComparison
);