namespace SpotTrack.Platform.Gyms.Domain.Model.ValueObjects;

public record BranchName
{
    public string Value { get; init; } = null!;

    private BranchName() { }

    public BranchName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("BranchName cannot be null or whitespace.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("BranchName cannot be longer than 100 characters.", nameof(value));
        Value = value;
    }
}
