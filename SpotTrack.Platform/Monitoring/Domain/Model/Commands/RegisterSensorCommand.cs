namespace SpotTrack.Platform.Monitoring.Domain.Model.Commands;

public record RegisterSensorCommand(
    int EquipmentId,
    string MacAddress,
    string Location,
    int BatteryLevel,
    int SignalStrength,
    string FirmwareVersion);
