namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

/// <param name="UserId">IAM user identifier — use this (not Id) for cross-context queries: memberships, reservations, routines.</param>
public record AdminResource(
    int Id,
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Dni);
