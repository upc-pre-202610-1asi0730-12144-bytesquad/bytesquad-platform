using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result> Handle(SignUpCommand command, CancellationToken cancellationToken);
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken);
}
