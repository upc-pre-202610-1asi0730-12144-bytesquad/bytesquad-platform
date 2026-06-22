namespace SpotTrack.Platform.Profiles.Domain.Model.Commands;

public record UpdateAdminProfileCommand(
    int AdminId,
    string FirstName,
    string LastName,
    string PhoneNumber);
