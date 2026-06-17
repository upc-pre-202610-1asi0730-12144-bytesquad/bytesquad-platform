using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Gyms.Domain.Model;
using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Domain.Repositories;
using SpotTrack.Platform.Gyms.Domain.Services;
using SpotTrack.Platform.Gyms.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Gyms.Application.Internal.CommandServices;

public class GymCommandService(
    IGymRepository gymRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<GymMessages> localizer)
    : IGymCommandService
{
    public async Task<Result<Gym>> Handle(CreateGymCommand command, CancellationToken cancellationToken)
    {
        Gym gym;
        try
        {
            gym = new Gym(command);
        }
        catch (ArgumentException)
        {
            return Result<Gym>.Failure(
                GymError.InvalidData,
                localizer[nameof(GymError.InvalidData)]);
        }

        try
        {
            await gymRepository.AddAsync(gym, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Gym>.Success(gym);
        }
        catch (OperationCanceledException)
        {
            return Result<Gym>.Failure(
                GymError.OperationCancelled,
                localizer[nameof(GymError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Gym>.Failure(
                GymError.DatabaseError,
                localizer[nameof(GymError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Gym>.Failure(
                GymError.InternalServerError,
                localizer[nameof(GymError.InternalServerError)]);
        }
    }

    public async Task<Result<Branch>> Handle(CreateBranchCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymRepository.FindByIdAsync(command.GymId, cancellationToken);
        if (gym is null)
            return Result<Branch>.Failure(
                GymError.GymNotFound,
                localizer[nameof(GymError.GymNotFound)]);

        try
        {
            gym.AddBranch(command.Name, command.Street, command.District, command.City);
        }
        catch (ArgumentException)
        {
            return Result<Branch>.Failure(
                GymError.InvalidData,
                localizer[nameof(GymError.InvalidData)]);
        }

        try
        {
            gymRepository.Update(gym);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Branch>.Success(gym.Branches.Last());
        }
        catch (OperationCanceledException)
        {
            return Result<Branch>.Failure(
                GymError.OperationCancelled,
                localizer[nameof(GymError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Branch>.Failure(
                GymError.DatabaseError,
                localizer[nameof(GymError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Branch>.Failure(
                GymError.InternalServerError,
                localizer[nameof(GymError.InternalServerError)]);
        }
    }
}
