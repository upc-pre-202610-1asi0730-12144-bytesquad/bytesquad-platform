using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Queries;

namespace SpotTrack.Platform.Profiles.Application.QueryServices;

public interface IClientQueryService
{
    Task<Client?> Handle(GetClientByIdQuery query, CancellationToken cancellationToken);
    Task<Client?> Handle(GetClientByEmailQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Client>> Handle(GetAllClientsQuery query, CancellationToken cancellationToken);
}
