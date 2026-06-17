using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Gyms.Domain.Services;

public interface IGymCommandService
{
    Task<Result<Gym>> Handle(CreateGymCommand command, CancellationToken cancellationToken);
    Task<Result<Branch>> Handle(CreateBranchCommand command, CancellationToken cancellationToken);
}
