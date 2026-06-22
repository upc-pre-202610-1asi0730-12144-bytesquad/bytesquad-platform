using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Queries;

namespace SpotTrack.Platform.Profiles.Application.QueryServices;

public interface IAdminQueryService
{
    Task<Admin?> Handle(GetAdminByIdQuery query, CancellationToken cancellationToken);
    Task<Admin?> Handle(GetAdminByEmailQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Admin>> Handle(GetAllAdminsQuery query, CancellationToken cancellationToken);
}
