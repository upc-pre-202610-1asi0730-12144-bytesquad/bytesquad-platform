using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result> Handle(SignUpCommand command, CancellationToken cancellationToken);
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken);
    Task<Result<User>> Handle(ProvisionIamAccountCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(ChangePasswordCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(VerifyForgotPasswordCommand command, CancellationToken cancellationToken);
    Task<Result<User>> Handle(UpdateNotificationPreferencesCommand command, CancellationToken cancellationToken);
}
