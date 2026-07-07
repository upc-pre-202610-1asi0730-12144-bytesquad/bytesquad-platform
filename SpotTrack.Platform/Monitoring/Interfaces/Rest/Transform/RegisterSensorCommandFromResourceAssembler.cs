using SpotTrack.Platform.Monitoring.Domain.Model.Commands;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;

public static class RegisterSensorCommandFromResourceAssembler
{
    public static RegisterSensorCommand ToCommandFromResource(RegisterSensorResource resource) =>
        new(resource.EquipmentId, resource.MacAddress, resource.Location,
            resource.BatteryLevel, resource.SignalStrength, resource.FirmwareVersion);
}
