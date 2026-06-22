using System.Text.RegularExpressions;

namespace SpotTrack.Platform.Profiles.Domain.Model.ValueObjects;

public record EmailAddress
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Address { get; init; }

    public EmailAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Email address cannot be null or empty.", nameof(address));
        if (!EmailRegex.IsMatch(address))
            throw new ArgumentException($"'{address}' is not a valid email address.", nameof(address));
        Address = address;
    }
}
