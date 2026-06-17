using SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

namespace SpotTrack.Platform.Routines.Domain.Model.Entities;

public class ExerciseBlock
{
    public int Id { get; private set; }

    public ExerciseName Name { get; private set; }

    public ExerciseType Type { get; private set; }

    public int Order { get; private set; }

    private ExerciseBlock() { Name = null!; Type = default; }

    public ExerciseBlock(ExerciseName name, ExerciseType type, int order)
    {
        if (order <= 0)
            throw new ArgumentException("Order cannot be zero or negative.", nameof(order));
        Name = name;
        Type = type;
        Order = order;
    }
}
