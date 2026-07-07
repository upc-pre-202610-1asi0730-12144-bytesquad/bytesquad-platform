using SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Routines.Domain.Model.Entities;

public class ExerciseBlock
{
    public int Id { get; private set; }

    public ExerciseName Name { get; private set; }

    public ExerciseType Type { get; private set; }

    public int Order { get; private set; }

    public int Sets { get; private set; }

    public int Reps { get; private set; }

    private ExerciseBlock() { Name = null!; Type = default; }

    public ExerciseBlock(ExerciseName name, ExerciseType type, int order, int sets, int reps)
    {
        if (order <= 0)
            throw new ArgumentException("Order cannot be zero or negative.", nameof(order));
        if (sets <= 0)
            throw new ArgumentException("Sets cannot be zero or negative.", nameof(sets));
        if (reps <= 0)
            throw new ArgumentException("Reps cannot be zero or negative.", nameof(reps));
        Name = name;
        Type = type;
        Order = order;
        Sets = sets;
        Reps = reps;
    }
}
