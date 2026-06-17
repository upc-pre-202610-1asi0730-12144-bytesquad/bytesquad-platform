using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Gym.Domain.Model;
using SpotTrack.Platform.Gym.Domain.Model.Aggregates;
using SpotTrack.Platform.Gym.Domain.Model.Commands;
using SpotTrack.Platform.Gym.Domain.Repositories;
using SpotTrack.Platform.Gym.Domain.Services;
using SpotTrack.Platform.Gym.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Gym.Application.Internal.CommandServices;

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
}
