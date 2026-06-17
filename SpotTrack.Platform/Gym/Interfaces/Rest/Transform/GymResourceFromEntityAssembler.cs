using SpotTrack.Platform.Gym.Domain.Model.Aggregates;
using SpotTrack.Platform.Gym.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gym.Interfaces.Rest.Transform;

public static class GymResourceFromEntityAssembler
{
    public static GymResource ToResourceFromEntity(Gym gym) =>
        new(gym.Id, gym.Name.Value, gym.Address.Street, gym.Address.District, gym.Address.City);
}
