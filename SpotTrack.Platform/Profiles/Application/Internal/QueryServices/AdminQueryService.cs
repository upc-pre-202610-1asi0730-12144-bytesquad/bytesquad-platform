using SpotTrack.Platform.Profiles.Application.QueryServices;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Queries;
using SpotTrack.Platform.Profiles.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Application.Internal.QueryServices;

public class AdminQueryService(IAdminRepository adminRepository) : IAdminQueryService
{
    public async Task<Admin?> Handle(GetAdminByIdQuery query, CancellationToken cancellationToken)
        => await adminRepository.FindByIdAsync(query.AdminId, cancellationToken);

    public async Task<Admin?> Handle(GetAdminByEmailQuery query, CancellationToken cancellationToken)
        => await adminRepository.FindByEmailAsync(query.Email, cancellationToken);

    public async Task<IEnumerable<Admin>> Handle(GetAllAdminsQuery query, CancellationToken cancellationToken)
        => await adminRepository.ListAsync(cancellationToken);
}
