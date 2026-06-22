using System.Text.RegularExpressions;

namespace SpotTrack.Platform.Profiles.Domain.Model.ValueObjects;

public record Dni
{
    private static readonly Regex DniRegex = new(@"^[0-9]{8}$", RegexOptions.Compiled);

    public string Value { get; init; }

    public Dni(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("DNI cannot be null or empty.", nameof(value));
        if (!DniRegex.IsMatch(value))
            throw new ArgumentException(
                $"'{value}' is not a valid Peruvian DNI. Must be exactly 8 digits.",
                nameof(value));
        Value = value;
    }
}
