namespace SpotTrack.Platform.Gym.Domain.Model.ValueObjects;

public record Address
{
    public string Street { get; init; } = null!;
    public string District { get; init; } = null!;
    public string City { get; init; } = null!;

    private Address() { }

    public Address(string street, string district, string city)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be null or whitespace.", nameof(street));
        if (string.IsNullOrWhiteSpace(district))
            throw new ArgumentException("District cannot be null or whitespace.", nameof(district));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be null or whitespace.", nameof(city));
        Street = street;
        District = district;
        City = city;
    }
}
