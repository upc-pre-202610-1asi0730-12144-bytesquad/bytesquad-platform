using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Gyms.Domain.Model.Events;

public record GymCreatedEvent(int GymId, int AdminId, string Name) : IEvent
{
    public static GymCreatedEvent FromGym(Gym gym) =>
        new(gym.Id, gym.AdminId, gym.Name.Value);
}
