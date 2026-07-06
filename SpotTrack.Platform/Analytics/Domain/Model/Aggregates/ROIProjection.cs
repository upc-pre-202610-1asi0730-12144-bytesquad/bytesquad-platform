using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Model.Aggregates;

public class ROIProjection
{
    public long Id { get; private set; }
    public ROIProjectionId RoiProjectionId { get; private set; } = null!;
    public int AdminId { get; private set; }
    public double ProjectedDowntimeCost { get; private set; }
    public double ProjectedEarnings { get; private set; }
    public double RoiIndex { get; private set; }
    public string DemandStatus { get; private set; } = null!;

    protected ROIProjection() { }

    public ROIProjection(RequestDowntimeCostProjectionCommand command)
    {
        AdminId = command.AuthenticatedAdminId;
        ProjectedDowntimeCost = command.ProjectedDowntimeCost;
        ProjectedEarnings = 0;
        RoiIndex = 0;
        DemandStatus = "UNDER_REVIEW";
    }

    public void InitializeId() => RoiProjectionId = new ROIProjectionId(Id);

    public void UpdateProjectedEarnings(double projectedEarnings)
    {
        ProjectedEarnings = projectedEarnings;
    }
    
    public void GenerateFinalProjection()
    {
        if (ProjectedDowntimeCost > 0)
        {
            RoiIndex = (ProjectedEarnings - ProjectedDowntimeCost) / ProjectedDowntimeCost;
        }
        else
        {
            RoiIndex = ProjectedEarnings;
        }

        DemandStatus = RoiIndex > 0.5 ? "HIGH_DEMAND" : "STABLE_DEMAND";
    }

}