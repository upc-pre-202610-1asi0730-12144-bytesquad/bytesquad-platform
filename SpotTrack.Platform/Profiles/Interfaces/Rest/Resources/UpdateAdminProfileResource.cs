namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

public record UpdateAdminProfileResource(
    string FirstName,
    string LastName,
    string PhoneNumber);
