namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

public record CreateAdminResource(
    int UserId,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Dni);
