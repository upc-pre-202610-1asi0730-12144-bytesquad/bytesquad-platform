namespace SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

public record ExerciseName
{
    public string Value { get; init; } = null!;
    
    public ExerciseName() { }

    public ExerciseName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ExerciseName cannot be null or whitespace.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("ExerciseName cannot be longer than 100 characters.", nameof(value));
        Value = value;
    }
}