namespace SpotTrack.Platform.Monitoring.Domain.Model.Entities;

public class SensorCapture
{
    public int Id { get; private set; }
    public DateTimeOffset DetectedAt { get; private set; }

    private SensorCapture() { }

    public SensorCapture(DateTimeOffset detectedAt)
    {
        DetectedAt = detectedAt;
    }
}
