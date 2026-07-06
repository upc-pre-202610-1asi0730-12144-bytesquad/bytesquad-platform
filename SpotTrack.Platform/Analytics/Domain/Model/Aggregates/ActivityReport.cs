using SpotTrack.Platform.Analytics.Domain.Model.Commands;
using SpotTrack.Platform.Analytics.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Analytics.Domain.Model.Aggregates
{
    public class ActivityReport
    {
        public long Id { get; private set; }
        public ActivityReportId ActivityReportId { get; private set; } = null!;
        public long TotalUsageTime { get; private set; }
        public long DowntimeCost { get; private set; }
        public double PercentageComparison { get; private set; }

        protected ActivityReport() { }

        public ActivityReport(RequestActivityAnalysisCommand command)
        {
            TotalUsageTime = command.TotalUsageTime;
            DowntimeCost = command.DowntimeCost;
            PercentageComparison = command.PercentageComparison;
        }

        public void InitializeId() => ActivityReportId = new ActivityReportId(Id);

        public void UpdateTotalUsageTime(long totalUsageTime) 
        {
            TotalUsageTime = totalUsageTime;
        }

        public void UpdateDowntimeCost(long downtimeCost)
        {
            DowntimeCost = downtimeCost;
        }

        public void UpdatePercentageComparison(double percentageComparison)
        {
            PercentageComparison = percentageComparison;
        }
    }
}