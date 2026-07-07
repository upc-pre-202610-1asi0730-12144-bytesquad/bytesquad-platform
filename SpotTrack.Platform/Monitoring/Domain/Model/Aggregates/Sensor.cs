using SpotTrack.Platform.Monitoring.Domain.Model.Commands;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;

public partial class Sensor
{
    public int Id { get; private set; }

    public int EquipmentId { get; private set; }

    public string MacAddress { get; private set; } = null!;

    public string Location { get; private set; } = null!;

    public ESensorStatus Status { get; private set; }

    public int BatteryLevel { get; private set; }

    public int SignalStrength { get; private set; }

    public string FirmwareVersion { get; private set; } = null!;

    public DateTimeOffset LastHeartbeat { get; private set; }

    public DateTimeOffset LastStatusChangeAt { get; private set; }

    private Sensor() { }

    public Sensor(RegisterSensorCommand command)
    {
        if (command.EquipmentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(command.EquipmentId), command.EquipmentId,
                "EquipmentId must be a positive integer.");
        if (string.IsNullOrWhiteSpace(command.MacAddress))
            throw new ArgumentException("MacAddress cannot be null or whitespace.", nameof(command.MacAddress));
        if (string.IsNullOrWhiteSpace(command.Location))
            throw new ArgumentException("Location cannot be null or whitespace.", nameof(command.Location));

        EquipmentId = command.EquipmentId;
        MacAddress = command.MacAddress;
        Location = command.Location;
        BatteryLevel = command.BatteryLevel;
        SignalStrength = command.SignalStrength;
        FirmwareVersion = command.FirmwareVersion;
        Status = ESensorStatus.Online;
        LastHeartbeat = DateTimeOffset.UtcNow;
        LastStatusChangeAt = DateTimeOffset.UtcNow;
    }

    // Simulates a network disconnection/reconnection — there is no real Edge hardware to report this.
    public void MarkDisconnected()
    {
        if (Status == ESensorStatus.Offline)
            throw new InvalidOperationException("Sensor is already marked as disconnected.");
        Status = ESensorStatus.Offline;
        LastStatusChangeAt = DateTimeOffset.UtcNow;
    }

    public void MarkReconnected()
    {
        if (Status == ESensorStatus.Online)
            throw new InvalidOperationException("Sensor is already marked as connected.");
        Status = ESensorStatus.Online;
        LastHeartbeat = DateTimeOffset.UtcNow;
        LastStatusChangeAt = DateTimeOffset.UtcNow;
    }
}
