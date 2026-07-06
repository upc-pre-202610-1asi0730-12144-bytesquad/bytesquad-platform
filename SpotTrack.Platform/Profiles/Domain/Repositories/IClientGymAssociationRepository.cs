using SpotTrack.Platform.Profiles.Domain.Model.Entities;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Domain.Repositories;

public interface IClientGymAssociationRepository : IBaseRepository<ClientGymAssociation>
{
    Task<bool> ExistsByClientIdAndGymIdAsync(int clientId, int gymId, CancellationToken cancellationToken = default);
    Task<ClientGymAssociation?> FindByClientIdAndGymIdAsync(int clientId, int gymId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClientGymAssociation>> FindAllByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClientGymAssociation>> FindAllByClientIdAndActiveAsync(int clientId, CancellationToken cancellationToken = default);
}
