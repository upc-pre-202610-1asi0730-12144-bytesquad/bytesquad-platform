namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record SensorResource(
    int Id,
    int EquipmentId,
    string MacAddress,
    string Location,
    string Status,
    int BatteryLevel,
    int SignalStrength,
    string FirmwareVersion,
    DateTimeOffset LastHeartbeat);
