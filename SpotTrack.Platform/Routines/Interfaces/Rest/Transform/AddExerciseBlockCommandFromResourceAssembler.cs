using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Routines.Interfaces.Rest.Transform;

public static class AddExerciseBlockCommandFromResourceAssembler
{
    public static AddExerciseBlockCommand ToCommandFromResource(int routineId, AddExerciseBlockResource resource) =>
        new(routineId, resource.ExerciseName, resource.ExerciseType, resource.Order);
}
