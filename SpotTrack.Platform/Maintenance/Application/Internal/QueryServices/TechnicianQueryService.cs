using SpotTrack.Platform.Maintenances.Application.QueryServices;
using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;
using SpotTrack.Platform.Maintenances.Domain.Repositories;

namespace SpotTrack.Platform.Maintenances.Application.Internal.QueryServices;

public class TechnicianQueryService(ITechnicianRepository technicianRepository)
    : ITechnicianQueryService
{
    public async Task<IEnumerable<Technician>> Handle(
        GetAllTechniciansByAdminIdQuery query,
        CancellationToken cancellationToken = default)
        => await technicianRepository.FindAllByAdminIdAsync(query.AdminId, cancellationToken);
}
