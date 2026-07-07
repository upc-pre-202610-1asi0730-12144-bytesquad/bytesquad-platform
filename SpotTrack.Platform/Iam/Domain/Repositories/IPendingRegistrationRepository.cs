using SpotTrack.Platform.Iam.Domain.Model.Aggregates;

namespace SpotTrack.Platform.Iam.Domain.Repositories;

public interface IPendingRegistrationRepository
{
    Task AddAsync(PendingRegistration registration, CancellationToken cancellationToken = default);
    Task<PendingRegistration?> FindByRegistrationIdAsync(Guid registrationId, CancellationToken cancellationToken = default);
    void Update(PendingRegistration registration);
}
