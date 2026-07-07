using SpotTrack.Platform.Gyms.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Gyms.Domain.Repositories;

public interface IEquipmentRepository : IBaseRepository<Equipment>
{
    Task<IEnumerable<Equipment>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Equipment>> FindAllByGymIdAsync(int gymId, CancellationToken cancellationToken = default);
}
