using System;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Model.Aggregates;

public class ROIProjection
{
    public long Id { get; private set; }
    public ROIProjectionId RoiProjectionId { get; private set; }
    public double ProjectedDowntimeCost { get; private set; }
    public double ProjectedEarnings { get; private set; }
    public double RoiIndex { get; private set; }
    public string DemandStatus { get; private set; }

    protected ROIProjection() { }

    public ROIProjection(RequestDowntimeCostProjectionCommand command)
    {
        RoiProjectionId = new ROIProjectionId(new Random().Next(1000, 100000));
        ProjectedDowntimeCost = command.ProjectedDowntimeCost;
        ProjectedEarnings = 0;
        RoiIndex = 0;
        DemandStatus = "UNDER_REVIEW";
    }

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