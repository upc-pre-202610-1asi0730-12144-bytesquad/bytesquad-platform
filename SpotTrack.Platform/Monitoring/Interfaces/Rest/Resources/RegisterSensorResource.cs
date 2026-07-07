namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

public record RegisterSensorResource(
    int EquipmentId,
    string MacAddress,
    string Location,
    int BatteryLevel,
    int SignalStrength,
    string FirmwareVersion);
