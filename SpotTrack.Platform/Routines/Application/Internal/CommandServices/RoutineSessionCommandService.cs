using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Routines.Application.CommandServices;
using SpotTrack.Platform.Routines.Domain.Model;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Domain.Repositories;
using SpotTrack.Platform.Routines.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Routines.Application.Internal.CommandServices;

public class RoutineSessionCommandService(
    IRoutineSessionRepository routineSessionRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<RoutinesMessages> localizer)
    : IRoutineSessionCommandService
{
    public async Task<Result<RoutineSession>> Handle(StartRoutineCommand command, CancellationToken cancellationToken)
    {
        RoutineSession session;
        try
        {
            session = new RoutineSession(command);
        }
        catch (ArgumentException)
        {
            return Result<RoutineSession>.Failure(
                RoutinesError.InvalidSessionData,
                localizer[nameof(RoutinesError.InvalidSessionData)]);
        }

        try
        {
            await routineSessionRepository.AddAsync(session, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<RoutineSession>.Success(session);
        }
        catch (OperationCanceledException)
        {
            return Result<RoutineSession>.Failure(
                RoutinesError.OperationCancelled,
                localizer[nameof(RoutinesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<RoutineSession>.Failure(
                RoutinesError.DatabaseError,
                localizer[nameof(RoutinesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<RoutineSession>.Failure(
                RoutinesError.InternalServerError,
                localizer[nameof(RoutinesError.InternalServerError)]);
        }
    }
}
