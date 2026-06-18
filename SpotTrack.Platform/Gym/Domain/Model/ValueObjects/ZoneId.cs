namespace SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

public record ZoneId
{
    public int Value { get; init; }

    private ZoneId() { }

    public ZoneId(int value)
    {
        if (value <= 0)
            throw new ArgumentException("ZoneId must be greater than zero.");
        Value = value;
    }
}
