namespace SpotTrack.Platform.Routines.Domain.Model.ValueObjects;

public record ClientId
{
    public int Value { get; init; }
    
    public ClientId() : this(0) { }

    public  ClientId(int value)
    {
        if (value <= 0)
            throw new ArgumentException("ClientId must be greater than zero.");
        Value = value;
    }
}