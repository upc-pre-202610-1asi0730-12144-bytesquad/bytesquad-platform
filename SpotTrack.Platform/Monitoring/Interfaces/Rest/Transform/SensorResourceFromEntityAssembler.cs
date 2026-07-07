using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;

public static class SensorResourceFromEntityAssembler
{
    public static SensorResource ToResourceFromEntity(Sensor sensor) =>
        new(sensor.Id, sensor.Type.ToString(), sensor.Identifier, sensor.AdminId, sensor.EquipmentId,
            sensor.RegisteredAt, sensor.Captures.Count);
}
