using SpotTrack.Platform.Monitoring.Domain.Model.Commands;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;

public class Anomaly
{
    public int Id { get; private set; }
    public int SensorId { get; private set; }
    public int AdminId { get; private set; }
    public string AnomalyType { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTimeOffset DetectedAt { get; private set; }

    private Anomaly() { }

    public Anomaly(ReportAnomalyCommand command, int adminId)
    {
        SensorId = command.SensorId;
        AdminId = adminId;
        AnomalyType = command.AnomalyType;
        Description = command.Description;
        DetectedAt = command.DetectedAt;
    }
}
