using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Memberships.Application.CommandServices;

public interface IBranchAccessCommandService
{
    Task<Result<BranchAccess>> Handle(CreateGrantBranchAccessCommand command, CancellationToken cancellationToken);
}
