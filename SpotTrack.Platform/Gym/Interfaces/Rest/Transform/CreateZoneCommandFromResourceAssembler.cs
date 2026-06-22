using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class CreateZoneCommandFromResourceAssembler
{
    public static CreateZoneCommand ToCommandFromResource(int gymId, int branchId, CreateZoneResource resource) =>
        new(gymId, branchId, resource.Name);
}
