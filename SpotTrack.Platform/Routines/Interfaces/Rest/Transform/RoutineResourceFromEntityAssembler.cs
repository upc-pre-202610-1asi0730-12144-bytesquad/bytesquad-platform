using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Routines.Interfaces.Rest.Transform;

public static class RoutineResourceFromEntityAssembler
{
    public static RoutineResource ToResourceFromEntity(Routine routine) =>
        new(routine.Id, routine.Name.Value, routine.ClientId.Value, routine.ExerciseBlocks.Count);
}
