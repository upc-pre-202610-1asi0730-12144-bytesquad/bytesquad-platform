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

    // Constructor para la Feature 9 (Inicia la proyección con el costo de inactividad)
    public ROIProjection(RequestDowntimeCostProjectionCommand command)
    {
        RoiProjectionId = new ROIProjectionId(new Random().Next(1000, 100000));
        ProjectedDowntimeCost = command.ProjectedDowntimeCost;
        ProjectedEarnings = 0;
        RoiIndex = 0;
        DemandStatus = "UNDER_REVIEW";
    }

    // Método de negocio para la Feature 10
    public void UpdateProjectedEarnings(double projectedEarnings)
    {
        ProjectedEarnings = projectedEarnings;
    }
}