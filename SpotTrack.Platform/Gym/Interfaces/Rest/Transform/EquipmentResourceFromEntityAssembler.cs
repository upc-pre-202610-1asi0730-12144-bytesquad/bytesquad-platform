using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class EquipmentResourceFromEntityAssembler
{
    public static EquipmentResource ToResourceFromEntity(Equipment equipment) =>
        new(equipment.Id, equipment.Name.Value, equipment.ZoneId.Value, equipment.Status.ToString());
}
