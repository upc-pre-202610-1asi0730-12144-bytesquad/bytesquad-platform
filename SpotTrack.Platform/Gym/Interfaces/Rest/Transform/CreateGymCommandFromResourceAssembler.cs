using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class CreateGymCommandFromResourceAssembler
{
    public static CreateGymCommand ToCommandFromResource(CreateGymResource resource) =>
        new(resource.Name, resource.Street, resource.District, resource.City);
}
