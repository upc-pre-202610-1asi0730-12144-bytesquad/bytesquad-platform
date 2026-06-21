using System;
using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Model.Aggregates
{
    public class ActivityReport
    {
        public long Id { get; private set; }
        public ActivityReportId ActivityReportId { get; private set; }
        public long TotalUsageTime { get; private set; }
        public long DowntimeCost { get; private set; }
        public double PercentageComparison { get; private set; }

        protected ActivityReport() { }

        public ActivityReport(RequestActivityAnalysisCommand command) 
        {
            ActivityReportId = new ActivityReportId(new Random().Next(1000, 100000));
            TotalUsageTime = command.TotalUsageTime;
            DowntimeCost = command.DowntimeCost;
            PercentageComparison = command.PercentageComparison;
        }

        // Método de negocio para la Feature 2
        public void UpdateTotalUsageTime(long totalUsageTime) 
        {
            TotalUsageTime = totalUsageTime;
        }
    }
}