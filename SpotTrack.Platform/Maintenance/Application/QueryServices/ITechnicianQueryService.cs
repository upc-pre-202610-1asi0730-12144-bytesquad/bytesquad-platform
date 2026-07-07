using SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;
using SpotTrack.Platform.Maintenances.Domain.Model.Queries;

namespace SpotTrack.Platform.Maintenances.Application.QueryServices;

public interface ITechnicianQueryService
{
    Task<IEnumerable<Technician>> Handle(GetAllTechniciansByAdminIdQuery query, CancellationToken cancellationToken = default);
}
