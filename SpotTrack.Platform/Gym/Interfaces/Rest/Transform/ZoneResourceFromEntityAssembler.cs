using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class ZoneResourceFromEntityAssembler
{
    public static ZoneResource ToResourceFromEntity(Zone zone) =>
        new(zone.Id, zone.Name.Value);
}
