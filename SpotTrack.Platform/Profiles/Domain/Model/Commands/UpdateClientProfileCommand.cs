namespace SpotTrack.Platform.Profiles.Domain.Model.Commands;

public record UpdateClientProfileCommand(
    int ClientId,
    string FirstName,
    string LastName,
    string PhoneNumber);
