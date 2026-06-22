namespace SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

public record EquipmentName
{
    public string Value { get; init; } = null!;

    private EquipmentName() { }

    public EquipmentName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("EquipmentName cannot be null or whitespace.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("EquipmentName cannot be longer than 100 characters.", nameof(value));
        Value = value;
    }
}
