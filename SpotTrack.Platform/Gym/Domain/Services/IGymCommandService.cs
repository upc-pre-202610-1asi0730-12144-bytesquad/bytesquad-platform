using SpotTrack.Platform.Gym.Domain.Model.Aggregates;
using SpotTrack.Platform.Gym.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Gym.Domain.Services;

public interface IGymCommandService
{
    Task<Result<Gym>> Handle(CreateGymCommand command, CancellationToken cancellationToken);
}
