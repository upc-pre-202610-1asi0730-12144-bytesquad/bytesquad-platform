using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class GymResourceFromEntityAssembler
{
    public static GymResource ToResourceFromEntity(Gym gym) =>
        new(gym.Id, gym.Name.Value, gym.Address.Street, gym.Address.District, gym.Address.City);
}
