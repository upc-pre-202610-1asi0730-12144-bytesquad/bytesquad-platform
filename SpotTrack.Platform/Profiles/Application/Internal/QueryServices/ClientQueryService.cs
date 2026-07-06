using SpotTrack.Platform.Profiles.Application.QueryServices;
using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Domain.Model.Entities;
using SpotTrack.Platform.Profiles.Domain.Model.Queries;
using SpotTrack.Platform.Profiles.Domain.Repositories;

namespace SpotTrack.Platform.Profiles.Application.Internal.QueryServices;

public class ClientQueryService(
    IClientRepository clientRepository,
    IClientGymAssociationRepository clientGymAssociationRepository)
    : IClientQueryService
{
    public async Task<Client?> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
        => await clientRepository.FindByIdAsync(query.ClientId, cancellationToken);

    public async Task<Client?> Handle(GetClientByEmailQuery query, CancellationToken cancellationToken)
        => await clientRepository.FindByEmailAsync(query.Email, cancellationToken);

    public async Task<Client?> Handle(GetClientByUserIdQuery query, CancellationToken cancellationToken)
        => await clientRepository.FindByUserIdAsync(query.UserId, cancellationToken);

    public async Task<IEnumerable<Client>> Handle(GetAllClientsQuery query, CancellationToken cancellationToken)
        => await clientRepository.ListAsync(cancellationToken);

    public async Task<IEnumerable<ClientGymAssociation>> Handle(
        GetClientGymAssociationsQuery query, CancellationToken cancellationToken)
        => await clientGymAssociationRepository.FindAllByClientIdAsync(query.ClientId, cancellationToken);
}
