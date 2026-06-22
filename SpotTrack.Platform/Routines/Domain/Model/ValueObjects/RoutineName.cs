namespace SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

public record RoutineName
{
    public string Value { get; init; } = null!;
    
    public RoutineName()  { }

    public RoutineName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("RoutineName cannot be null or whitespace.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("RoutineName cannot be longer than 100 characters.", nameof(value));
        Value = value;
    }
}