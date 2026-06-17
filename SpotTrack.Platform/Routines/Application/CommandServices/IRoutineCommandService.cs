using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Domain.Model.Entities;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Routines.Application.CommandServices;

public interface IRoutineCommandService
{
    Task<Result<Routine>> Handle(CreateRoutineCommand command, CancellationToken cancellationToken);
    Task<Result<ExerciseBlock>> Handle(AddExerciseBlockCommand command, CancellationToken cancellationToken);
}
