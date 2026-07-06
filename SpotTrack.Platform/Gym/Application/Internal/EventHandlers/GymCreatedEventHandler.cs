using SpotTrack.Platform.Gyms.Domain.Model.Events;
using SpotTrack.Platform.Shared.Application.Internal.EventHandlers;

namespace SpotTrack.Platform.Gyms.Application.Internal.EventHandlers;

// Ready to be consumed by Analytics or other BCs when needed.
public class GymCreatedEventHandler : IEventHandler<GymCreatedEvent>
{
    public Task Handle(GymCreatedEvent notification, CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
