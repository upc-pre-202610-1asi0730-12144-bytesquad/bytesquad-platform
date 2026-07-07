using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Gyms.Domain.Repositories;

public interface IAuthorizedDniRepository : IBaseRepository<AuthorizedDni>
{
    Task<bool> ExistsByGymIdAndDniAsync(int gymId, string dni, CancellationToken cancellationToken = default);
    Task<AuthorizedDni?> FindByGymIdAndDniAsync(int gymId, string dni, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuthorizedDni>> FindAllByGymIdAsync(int gymId, CancellationToken cancellationToken = default);
}
