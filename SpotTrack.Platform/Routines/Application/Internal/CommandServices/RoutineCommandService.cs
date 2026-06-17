using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SpotTrack.Platform.Routines.Application.CommandServices;
using SpotTrack.Platform.Routines.Domain.Model;
using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Domain.Model.Entities;
using SpotTrack.Platform.Routines.Domain.Repositories;
using SpotTrack.Platform.Routines.Resources;
using SpotTrack.Platform.Shared.Application.Model;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Routines.Application.Internal.CommandServices;

public class RoutineCommandService(
    IRoutineRepository routineRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IStringLocalizer<RoutinesMessages> localizer)
    : IRoutineCommandService
{
    public async Task<Result<Routine>> Handle(CreateRoutineCommand command, CancellationToken cancellationToken)
    {
        Routine routine;
        try
        {
            routine = new Routine(command);
        }
        catch (ArgumentException)
        {
            return Result<Routine>.Failure(
                RoutinesError.InvalidRoutineData,
                localizer[nameof(RoutinesError.InvalidRoutineData)]);
        }

        try
        {
            await routineRepository.AddAsync(routine, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Routine>.Success(routine);
        }
        catch (OperationCanceledException)
        {
            return Result<Routine>.Failure(
                RoutinesError.OperationCancelled,
                localizer[nameof(RoutinesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<Routine>.Failure(
                RoutinesError.DatabaseError,
                localizer[nameof(RoutinesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<Routine>.Failure(
                RoutinesError.InternalServerError,
                localizer[nameof(RoutinesError.InternalServerError)]);
        }
    }

    public async Task<Result<ExerciseBlock>> Handle(AddExerciseBlockCommand command, CancellationToken cancellationToken)
    {
        var routine = await routineRepository.FindByIdAsync(command.RoutineId, cancellationToken);
        if (routine is null)
            return Result<ExerciseBlock>.Failure(
                RoutinesError.RoutineNotFound,
                localizer[nameof(RoutinesError.RoutineNotFound)]);

        try
        {
            routine.AddExerciseBlock(command.ExerciseName, command.ExerciseType, command.Order);
        }
        catch (ArgumentException)
        {
            return Result<ExerciseBlock>.Failure(
                RoutinesError.InvalidExerciseData,
                localizer[nameof(RoutinesError.InvalidExerciseData)]);
        }

        try
        {
            routineRepository.Update(routine);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<ExerciseBlock>.Success(routine.ExerciseBlocks.Last());
        }
        catch (OperationCanceledException)
        {
            return Result<ExerciseBlock>.Failure(
                RoutinesError.OperationCancelled,
                localizer[nameof(RoutinesError.OperationCancelled)]);
        }
        catch (DbUpdateException)
        {
            return Result<ExerciseBlock>.Failure(
                RoutinesError.DatabaseError,
                localizer[nameof(RoutinesError.DatabaseError)]);
        }
        catch (Exception)
        {
            return Result<ExerciseBlock>.Failure(
                RoutinesError.InternalServerError,
                localizer[nameof(RoutinesError.InternalServerError)]);
        }
    }
}
