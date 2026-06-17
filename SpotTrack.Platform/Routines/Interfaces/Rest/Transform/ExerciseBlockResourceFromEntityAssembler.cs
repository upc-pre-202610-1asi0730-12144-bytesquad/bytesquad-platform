using SpotTrack.Platform.Routines.Domain.Model.Entities;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Routines.Interfaces.Rest.Transform;

public static class ExerciseBlockResourceFromEntityAssembler
{
    public static ExerciseBlockResource ToResourceFromEntity(ExerciseBlock exerciseBlock) =>
        new(exerciseBlock.Id, exerciseBlock.Name.Value, exerciseBlock.Type.ToString(), exerciseBlock.Order);
}
