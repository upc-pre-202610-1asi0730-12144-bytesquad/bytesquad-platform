namespace SpotTrack.Platform.Iam.Domain.Model.Commands;

public record SavePendingRegistrationCommand(
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
