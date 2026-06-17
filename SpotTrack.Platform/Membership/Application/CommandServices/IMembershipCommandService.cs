using SpotTrack.Platform.Memberships.Domain.Model.Aggregates;
using SpotTrack.Platform.Memberships.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Memberships.Application.CommandServices;

public interface IMembershipCommandService
{
    Task<Result<Membership>> Handle(CreateActivateMembershipCommand command, CancellationToken cancellationToken);
    Task<Result<Membership>> Handle(CreateUpgradeMembershipPlanCommand command, CancellationToken cancellationToken);
}
