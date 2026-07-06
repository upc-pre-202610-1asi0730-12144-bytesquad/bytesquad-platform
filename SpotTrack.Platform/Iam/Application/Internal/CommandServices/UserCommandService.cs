using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Iam.Application.CommandServices;
using SpotTrack.Platform.Iam.Application.Internal.OutboundServices;
using SpotTrack.Platform.Iam.Domain.Model;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Domain.Model.ValueObjects;
using SpotTrack.Platform.Iam.Domain.Repositories;
using SpotTrack.Platform.Iam.Resources;
using SpotTrack.Platform.Profiles.Interfaces.Acl;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Iam.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork,
    IProfilesContextFacade profilesFacade,
    IStringLocalizer<IamMessages> localizer)
    : IUserCommandService
{
    public async Task<Result> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByUsernameAsync(command.Username, cancellationToken))
            return Result.Failure(
                IamError.UsernameAlreadyTaken,
                localizer[nameof(IamError.UsernameAlreadyTaken), command.Username]);

        var passwordHash = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, passwordHash, UserRole.Client);

        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return Result.Failure(
                IamError.OperationCancelled,
                localizer[nameof(IamError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result.Failure(
                IamError.DatabaseError,
                localizer[nameof(IamError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result.Failure(
                IamError.InternalServerError,
                localizer[nameof(IamError.InternalServerError)]);
        }

        await profilesFacade.RegisterClientAsync(user.Id);

        return Result.Success();
    }

    public async Task<Result<User>> Handle(ProvisionIamAccountCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByUsernameAsync(command.Email, cancellationToken))
            return Result<User>.Failure(
                IamError.UsernameAlreadyTaken,
                localizer[nameof(IamError.UsernameAlreadyTaken), command.Email]);

        var user = new User(command.Email, command.AlreadyHashedPassword, UserRole.Admin);

        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (OperationCanceledException)
        {
            return Result<User>.Failure(
                IamError.OperationCancelled,
                localizer[nameof(IamError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<User>.Failure(
                IamError.DatabaseError,
                localizer[nameof(IamError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<User>.Failure(
                IamError.InternalServerError,
                localizer[nameof(IamError.InternalServerError)]);
        }
    }

    public async Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username, cancellationToken);
        if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User, string)>.Failure(
                IamError.InvalidCredentials,
                localizer[nameof(IamError.InvalidCredentials)]);

        try
        {
            var token = tokenService.GenerateToken(user);
            return Result<(User, string)>.Success((user, token));
        }
        catch (OperationCanceledException)
        {
            return Result<(User, string)>.Failure(
                IamError.OperationCancelled,
                localizer[nameof(IamError.OperationCancelled)]);
        }
        catch (Exception)
        {
            return Result<(User, string)>.Failure(
                IamError.InternalServerError,
                localizer[nameof(IamError.InternalServerError)]);
        }
    }
}
