namespace SpotTrack.Platform.Routines.Domain.Model.Entities;

public class SessionExerciseCompletion
{
    public int Id { get; private set; }

    public int ExerciseBlockId { get; private set; }

    private SessionExerciseCompletion() { }

    public SessionExerciseCompletion(int exerciseBlockId)
    {
        ExerciseBlockId = exerciseBlockId;
    }
}
