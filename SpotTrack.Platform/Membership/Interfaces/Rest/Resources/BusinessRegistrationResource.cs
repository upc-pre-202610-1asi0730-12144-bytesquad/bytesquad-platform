namespace SpotTrack.Platform.Memberships.Interfaces.Rest.Resources;

public record BusinessRegistrationResource(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Dni,
    string CompanyName,
    string Ruc,
    string LegalStructure,
    string CompanyPhone,
    string CompanyEmail,
    string StreetAddress,
    string City,
    string District,
    string MembershipTier);
