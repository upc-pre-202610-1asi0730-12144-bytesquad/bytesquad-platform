namespace SpotTrack.Platform.Analytics.Interfaces.REST.Resources;

public record ROIProjectionResource(
    long Id,
    long RoiProjectionId,
    double ProjectedDowntimeCost,
    double ProjectedEarnings,
    double RoiIndex,
    string DemandStatus
);