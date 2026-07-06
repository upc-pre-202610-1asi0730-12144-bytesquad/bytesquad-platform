using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Profiles.Application.CommandServices;

public interface IBusinessCommandService
{
    Task<Result<Business>> Handle(ProvisionBusinessCommand command, CancellationToken cancellationToken);
}
