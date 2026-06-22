using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Profiles.Application.CommandServices;
using SpotTrack.Platform.Profiles.Domain.Model;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Domain.Model.Events;
using SpotTrack.Platform.Profiles.Domain.Repositories;
using SpotTrack.Platform.Profiles.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Application.Internal.CommandServices;

public class AdminCommandService(
    IAdminRepository adminRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<ProfilesMessages> localizer)
    : IAdminCommandService
{
    public async Task<Result<Admin>> Handle(CreateAdminCommand command, CancellationToken cancellationToken)
    {
        if (await adminRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<Admin>.Failure(
                ProfilesError.EmailAlreadyRegistered,
                localizer[nameof(ProfilesError.EmailAlreadyRegistered), command.Email]);

        Admin admin;
        try
        {
            admin = new Admin(command);
        }
        catch (ArgumentException)
        {
            return Result<Admin>.Failure(
                ProfilesError.InvalidProfileData,
                localizer[nameof(ProfilesError.InvalidProfileData)]);
        }

        try
        {
            await adminRepository.AddAsync(admin, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            await mediator.PublishAsync(AdminRegisteredEvent.FromAdmin(admin), cancellationToken);
            return Result<Admin>.Success(admin);
        }
        catch (OperationCanceledException)
        {
            return Result<Admin>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Admin>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Admin>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }

    public async Task<Result<Admin>> Handle(RegisterAdminCommand command, CancellationToken cancellationToken)
    {
        var admin = new Admin(command);
        try
        {
            await adminRepository.AddAsync(admin, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Admin>.Success(admin);
        }
        catch (OperationCanceledException)
        {
            return Result<Admin>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Admin>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Admin>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }

    public async Task<Result<Admin>> Handle(UpdateAdminProfileCommand command, CancellationToken cancellationToken)
    {
        var admin = await adminRepository.FindByIdAsync(command.AdminId, cancellationToken);
        if (admin is null)
            return Result<Admin>.Failure(
                ProfilesError.AdminNotFound,
                localizer[nameof(ProfilesError.AdminNotFound)]);

        try
        {
            admin.UpdateProfile(command);
        }
        catch (ArgumentException)
        {
            return Result<Admin>.Failure(
                ProfilesError.InvalidProfileData,
                localizer[nameof(ProfilesError.InvalidProfileData)]);
        }

        try
        {
            adminRepository.Update(admin);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Admin>.Success(admin);
        }
        catch (OperationCanceledException)
        {
            return Result<Admin>.Failure(
                ProfilesError.OperationCancelled,
                localizer[nameof(ProfilesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Admin>.Failure(
                ProfilesError.DatabaseError,
                localizer[nameof(ProfilesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Admin>.Failure(
                ProfilesError.InternalServerError,
                localizer[nameof(ProfilesError.InternalServerError)]);
        }
    }
}
