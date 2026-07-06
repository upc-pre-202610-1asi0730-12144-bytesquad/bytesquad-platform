using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Iam.Application.CommandServices;
using SpotTrack.Platform.Iam.Application.Internal.OutboundServices;
using SpotTrack.Platform.Iam.Domain.Model;
using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Domain.Repositories;
using SpotTrack.Platform.Iam.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Iam.Application.Internal.CommandServices;

public class PendingRegistrationCommandService(
    IPendingRegistrationRepository pendingRegistrationRepository,
    IUserRepository userRepository,
    IHashingService hashingService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<IamMessages> localizer)
    : IPendingRegistrationCommandService
{
    public async Task<Result<Guid>> Handle(SavePendingRegistrationCommand command, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByUsernameAsync(command.Email, cancellationToken))
            return Result<Guid>.Failure(
                IamError.PendingRegistrationEmailAlreadyRegistered,
                localizer[nameof(IamError.PendingRegistrationEmailAlreadyRegistered), command.Email]);

        var hashedPassword = hashingService.HashPassword(command.Password);

        PendingRegistration registration;
        try
        {
            registration = new PendingRegistration(command, hashedPassword);
        }
        catch (ArgumentException)
        {
            return Result<Guid>.Failure(
                IamError.InternalServerError,
                localizer[nameof(IamError.InternalServerError)]);
        }

        try
        {
            await pendingRegistrationRepository.AddAsync(registration, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Guid>.Success(registration.RegistrationId);
        }
        catch (OperationCanceledException)
        {
            return Result<Guid>.Failure(
                IamError.OperationCancelled,
                localizer[nameof(IamError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Guid>.Failure(
                IamError.DatabaseError,
                localizer[nameof(IamError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Guid>.Failure(
                IamError.InternalServerError,
                localizer[nameof(IamError.InternalServerError)]);
        }
    }
}
