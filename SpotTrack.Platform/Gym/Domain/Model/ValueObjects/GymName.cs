namespace SpotTrack.Platform.Gym.Domain.Model.ValueObjects;

public record GymName
{
    public string Value { get; init; } = null!;

    private GymName() { }

    public GymName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("GymName cannot be null or whitespace.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("GymName cannot be longer than 100 characters.", nameof(value));
        Value = value;
    }
}
