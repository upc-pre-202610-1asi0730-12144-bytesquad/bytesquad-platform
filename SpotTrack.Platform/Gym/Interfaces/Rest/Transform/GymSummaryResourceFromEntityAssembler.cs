using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class GymSummaryResourceFromEntityAssembler
{
    public static GymSummaryResource ToResourceFromEntity(Gym gym) =>
        new(gym.Id, gym.Name.Value);
}
