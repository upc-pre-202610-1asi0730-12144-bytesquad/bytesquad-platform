namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

public record UpdateClientProfileResource(
    string FirstName,
    string LastName,
    string PhoneNumber);
