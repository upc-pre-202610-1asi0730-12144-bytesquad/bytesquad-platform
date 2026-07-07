using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Domain.Model.Entities;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;

public class Sensor
{
    public int Id { get; private set; }
    public SensorType Type { get; private set; }
    public string Identifier { get; private set; } = null!;
    public int AdminId { get; private set; }
    public int? EquipmentId { get; private set; }
    public DateTimeOffset RegisteredAt { get; private set; }
    public ICollection<SensorCapture> Captures { get; private set; } = new List<SensorCapture>();

    private Sensor() { }

    public Sensor(RegisterSensorCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Identifier))
            throw new ArgumentException("Sensor identifier is required.", nameof(command.Identifier));
        if (command.AdminId <= 0)
            throw new ArgumentException("AdminId must be positive.", nameof(command.AdminId));

        Type = command.SensorType;
        Identifier = command.Identifier;
        AdminId = command.AdminId;
        EquipmentId = command.EquipmentId;
        RegisteredAt = DateTimeOffset.UtcNow;
    }

    public void CaptureEvent(DateTimeOffset detectedAt)
    {
        Captures.Add(new SensorCapture(detectedAt));
    }
}
