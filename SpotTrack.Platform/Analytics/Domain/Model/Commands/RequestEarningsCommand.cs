namespace SpotTrack.Platform.Analytics.Domain.Model.Commands;

public record RequestEarningsProjectionCommand(long RoiProjectionId, double ProjectedEarnings);
