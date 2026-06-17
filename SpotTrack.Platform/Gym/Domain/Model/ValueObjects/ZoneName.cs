namespace SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

public record ZoneName
{
    public string Value { get; init; } = null!;

    private ZoneName() { }

    public ZoneName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ZoneName cannot be null or whitespace.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("ZoneName cannot be longer than 100 characters.", nameof(value));
        Value = value;
    }
}
