using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class RegisterEquipmentCommandFromResourceAssembler
{
    public static RegisterEquipmentCommand ToCommandFromResource(RegisterEquipmentResource resource) =>
        new(resource.Name, resource.ZoneId);
}
