namespace SpotTrack.Platform.Profiles.Domain.Model.ValueObjects;

public record PersonName
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string FullName => $"{FirstName} {LastName}";

    public PersonName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        FirstName = firstName;
        LastName = lastName;
    }
}
