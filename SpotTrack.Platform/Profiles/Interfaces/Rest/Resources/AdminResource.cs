namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

public record AdminResource(
    int Id,
    int UserId,
    string FullName,
    string Email,
    string PhoneNumber,
    string Dni);
