using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Iam.Application.CommandServices;

public interface IPendingRegistrationCommandService
{
    Task<Result<Guid>> Handle(SavePendingRegistrationCommand command, CancellationToken cancellationToken);
}
