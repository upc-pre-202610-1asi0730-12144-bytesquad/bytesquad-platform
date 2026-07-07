using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Domain.Repositories;

public interface ITechnicianRepository : IBaseRepository<Technician>
{
    Task<IEnumerable<Technician>> FindAllByAdminIdAsync(int adminId, CancellationToken cancellationToken = default);
}
