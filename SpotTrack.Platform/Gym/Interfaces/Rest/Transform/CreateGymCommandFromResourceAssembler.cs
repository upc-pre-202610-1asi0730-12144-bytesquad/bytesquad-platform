using SpotTrack.Platform.Gym.Domain.Model.Commands;
using SpotTrack.Platform.Gym.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gym.Interfaces.Rest.Transform;

public static class CreateGymCommandFromResourceAssembler
{
    public static CreateGymCommand ToCommandFromResource(CreateGymResource resource) =>
        new(resource.Name, resource.Street, resource.District, resource.City);
}
