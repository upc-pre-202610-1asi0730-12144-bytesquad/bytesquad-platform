using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Shared.Application.Model;

namespace SpotTrack.Platform.Iam.Interfaces.Acl;

public record PendingRegistrationData(
    string Email,
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

public interface IIamContextFacade
{
    Task<int> CreateUserAsync(string username, string password);
    Task<int> FetchUserIdByUsernameAsync(string username);
    Task<string> FetchUsernameByUserIdAsync(int userId);
    Task<Result<Guid>> SavePendingRegistrationAsync(SavePendingRegistrationCommand command, CancellationToken cancellationToken = default);
    Task<int> ConsumePendingRegistrationAsync(Guid registrationId, CancellationToken cancellationToken = default);
    Task<PendingRegistrationData?> FetchPendingRegistrationAsync(Guid registrationId, CancellationToken cancellationToken = default);
}
