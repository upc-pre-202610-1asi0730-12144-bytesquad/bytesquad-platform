namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

public record ClientResource(
    int Id,
    int UserId,
    string FullName,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Dni);
