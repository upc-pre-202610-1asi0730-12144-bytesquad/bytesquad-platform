using Microsoft.Extensions.Logging;
using SpotTrack.Platform.Iam.Application.CommandServices;
using SpotTrack.Platform.Iam.Application.QueryServices;
using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Domain.Model.Queries;
using SpotTrack.Platform.Iam.Domain.Repositories;
using SpotTrack.Platform.Iam.Interfaces.Acl;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Iam.Application.Acl;

public class IamContextFacade(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    IPendingRegistrationCommandService pendingRegistrationCommandService,
    IPendingRegistrationRepository pendingRegistrationRepository,
    ILogger<IamContextFacade> logger)
    : IIamContextFacade
{
    public async Task<int> CreateUserAsync(string username, string password)
    {
        var command = new SignUpCommand(username, password);
        var result = await userCommandService.Handle(command, CancellationToken.None);
        if (result.IsFailure) return 0;
        var user = await userQueryService.Handle(new GetUserByUsernameQuery(username), CancellationToken.None);
        return user?.Id ?? 0;
    }

    public async Task<int> FetchUserIdByUsernameAsync(string username)
    {
        var user = await userQueryService.Handle(new GetUserByUsernameQuery(username), CancellationToken.None);
        return user?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserIdAsync(int userId)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId), CancellationToken.None);
        return user?.Username ?? string.Empty;
    }

    public async Task<Result<Guid>> SavePendingRegistrationAsync(SavePendingRegistrationCommand command, CancellationToken cancellationToken = default) =>
        await pendingRegistrationCommandService.Handle(command, cancellationToken);

    public async Task<int> ConsumePendingRegistrationAsync(Guid registrationId, CancellationToken cancellationToken = default)
    {
        var registration = await pendingRegistrationRepository.FindByRegistrationIdAsync(registrationId, cancellationToken);
        if (registration is null)
        {
            logger.LogWarning("ConsumePendingRegistration: no pending registration found for id {RegistrationId}", registrationId);
            return 0;
        }

        if (registration.Status == Domain.Model.ValueObjects.PendingRegistrationStatus.Consumed)
        {
            logger.LogInformation("ConsumePendingRegistration: registration {RegistrationId} already consumed, returning existing user", registrationId);
            var existingUser = await userQueryService.Handle(new GetUserByUsernameQuery(registration.Email), cancellationToken);
            if (existingUser is null)
            {
                logger.LogError("ConsumePendingRegistration: registration {RegistrationId} is Consumed but no IAM user found for email {Email}", registrationId, registration.Email);
                return 0;
            }
            return existingUser.Id;
        }

        if (registration.IsExpired())
        {
            logger.LogWarning("ConsumePendingRegistration: registration {RegistrationId} has expired", registrationId);
            return 0;
        }

        var alreadyExists = await userQueryService.Handle(new GetUserByUsernameQuery(registration.Email), cancellationToken);
        if (alreadyExists is not null)
        {
            logger.LogWarning("ConsumePendingRegistration: IAM user already exists for {Email} — marking registration {RegistrationId} as consumed", registration.Email, registrationId);
            registration.Consume();
            pendingRegistrationRepository.Update(registration);
            return alreadyExists.Id;
        }

        var provisionCommand = new ProvisionIamAccountCommand(registration.Email, registration.HashedPassword);
        var result = await userCommandService.Handle(provisionCommand, cancellationToken);
        if (result.IsFailure)
        {
            logger.LogError("ConsumePendingRegistration: failed to provision IAM account for registration {RegistrationId}: {Message}", registrationId, result.Message);
            return 0;
        }

        registration.Consume();
        pendingRegistrationRepository.Update(registration);

        logger.LogInformation("ConsumePendingRegistration: provisioned IAM user {UserId} for registration {RegistrationId}", result.Value!.Id, registrationId);
        return result.Value!.Id;
    }

    public async Task<PendingRegistrationData?> FetchPendingRegistrationAsync(Guid registrationId, CancellationToken cancellationToken = default)
    {
        var registration = await pendingRegistrationRepository.FindByRegistrationIdAsync(registrationId, cancellationToken);
        if (registration is null) return null;

        return new PendingRegistrationData(
            registration.Email,
            registration.FirstName,
            registration.LastName,
            registration.PhoneNumber,
            registration.Dni,
            registration.CompanyName,
            registration.Ruc,
            registration.LegalStructure,
            registration.CompanyPhone,
            registration.CompanyEmail,
            registration.StreetAddress,
            registration.City,
            registration.District,
            registration.MembershipTier);
    }
}
