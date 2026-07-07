namespace SpotTrack.Platform.Routines.Domain.Model.Entities;

public class SessionBlockCompletion
{
    public int Id { get; private set; }
    public int ExerciseBlockId { get; private set; }

    private SessionBlockCompletion() { }

    public SessionBlockCompletion(int exerciseBlockId)
    {
        ExerciseBlockId = exerciseBlockId;
    }
}
