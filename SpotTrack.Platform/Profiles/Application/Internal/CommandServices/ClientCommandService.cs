using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Profiles.Application.CommandServices;
using SpotTrack.Platform.Profiles.Domain.Model;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Domain.Model.Entities;
using SpotTrack.Platform.Profiles.Domain.Model.Events;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Profiles.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Application.Internal.CommandServices;

public class ClientCommandService(
    IClientRepository clientRepository,
    IClientGymAssociationRepository clientGymAssociationRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<ProfilesMessages> localizer)
    : IClientCommandService
{
    public async Task<Result<Client>> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        if (await clientRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<Client>.Failure(
                ProfilesError.EmailAlreadyRegistered,
                localizer[nameof(ProfilesError.EmailAlreadyRegistered), command.Email]);

        Client client;
        try
        {
            client = new Client(command);
        }
        catch (ArgumentException)
        {
            return Result<Client>.Failure(
                ProfilesError.InvalidProfileData,
                localizer[nameof(ProfilesError.InvalidProfileData)]);
        }

        try
        {
            await clientRepository.AddAsync(client, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await mediator.PublishAsync(ClientRegisteredEvent.FromClient(client), cancellationToken);
            return Result<Client>.Success(client);
        }
        catch (OperationCanceledException)
        {
            return Result<Client>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Client>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Client>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }

    public async Task<Result<Client>> Handle(RegisterClientCommand command, CancellationToken cancellationToken)
    {
        var client = new Client(command);
        try
        {
            await clientRepository.AddAsync(client, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Client>.Success(client);
        }
        catch (OperationCanceledException)
        {
            return Result<Client>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Client>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Client>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }

    public async Task<Result<Client>> Handle(UpdateClientProfileCommand command, CancellationToken cancellationToken)
    {
        var client = await clientRepository.FindByIdAsync(command.ClientId, cancellationToken);
        if (client is null)
            return Result<Client>.Failure(
                ProfilesError.ClientNotFound,
                localizer[nameof(ProfilesError.ClientNotFound)]);

        try
        {
            client.UpdateProfile(command);
        }
        catch (ArgumentException)
        {
            return Result<Client>.Failure(
                ProfilesError.InvalidProfileData,
                localizer[nameof(ProfilesError.InvalidProfileData)]);
        }

        try
        {
            clientRepository.Update(client);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Client>.Success(client);
        }
        catch (OperationCanceledException)
        {
            return Result<Client>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Client>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Client>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }

    public async Task<Result<ClientGymAssociation>> Handle(
        AssociateClientWithGymCommand command, CancellationToken cancellationToken)
    {
        var client = await clientRepository.FindByIdAsync(command.ClientId, cancellationToken);
        if (client is null)
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.ClientNotFound,
                localizer[nameof(ProfilesError.ClientNotFound)]);

        if (!client.IsProfileComplete())
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.ProfileIncomplete,
                localizer[nameof(ProfilesError.ProfileIncomplete)]);

        // TODO: once Gym exposes IGymContextFacade.IsDniWhitelistedForGymAsync(gymId, dni),
        // reject the association here if the client's DNI is not whitelisted for the gym.

        if (await clientGymAssociationRepository.ExistsByClientIdAndGymIdAsync(
                command.ClientId, command.GymId, cancellationToken))
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.AlreadyAssociatedWithGym,
                localizer[nameof(ProfilesError.AlreadyAssociatedWithGym)]);

        var existingAssociations = await clientGymAssociationRepository.FindAllByClientIdAsync(
            command.ClientId, cancellationToken);
        var isFirst = !existingAssociations.Any();

        var association = ClientGymAssociation.Create(command.ClientId, command.GymId, isFirst);

        try
        {
            await clientGymAssociationRepository.AddAsync(association, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ClientGymAssociation>.Success(association);
        }
        catch (OperationCanceledException)
        {
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }

    public async Task<Result<ClientGymAssociation>> Handle(
        ChangeActiveGymCommand command, CancellationToken cancellationToken)
    {
        var target = await clientGymAssociationRepository.FindByClientIdAndGymIdAsync(
            command.ClientId, command.GymId, cancellationToken);
        if (target is null)
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.GymAssociationNotFound,
                localizer[nameof(ProfilesError.GymAssociationNotFound)]);

        // TODO: once Gym exposes IGymContextFacade.IsDniWhitelistedForGymAsync(gymId, dni),
        // reject the switch here if the client's DNI is no longer whitelisted for the gym.

        try
        {
            var currentlyActive = await clientGymAssociationRepository.FindAllByClientIdAndActiveAsync(
                command.ClientId, cancellationToken);
            foreach (var association in currentlyActive)
            {
                association.Deactivate();
                clientGymAssociationRepository.Update(association);
            }

            target.Activate();
            clientGymAssociationRepository.Update(target);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ClientGymAssociation>.Success(target);
        }
        catch (OperationCanceledException)
        {
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<ClientGymAssociation>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }
}
