using SpotTrack.Platform.Profiles.Domain.Model.Entities;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class ClientGymAssociationResourceFromEntityAssembler
{
    public static ClientGymAssociationResource ToResourceFromEntity(ClientGymAssociation entity)
        => new(entity.Id, entity.ClientId, entity.GymId, entity.Active);
}
