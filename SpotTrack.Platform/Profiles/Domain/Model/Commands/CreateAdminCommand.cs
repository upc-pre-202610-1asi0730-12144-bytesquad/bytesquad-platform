namespace SpotTrack.Platform.Profiles.Domain.Model.Commands;

public record CreateAdminCommand(
    int UserId,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Dni);
