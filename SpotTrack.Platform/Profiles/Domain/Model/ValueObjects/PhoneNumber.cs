using System.Text.RegularExpressions;

namespace SpotTrack.Platform.Profiles.Domain.Model.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex PhoneRegex = new(@"^\+?[0-9]{9,15}$", RegexOptions.Compiled);

    public string Number { get; init; }

    public PhoneNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Phone number cannot be null or empty.", nameof(number));
        if (!PhoneRegex.IsMatch(number))
            throw new ArgumentException(
                $"'{number}' is not a valid phone number. Must be 9-15 digits, optionally starting with '+'.",
                nameof(number));
        Number = number;
    }
}
