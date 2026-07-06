using SpotTrack.Platform.Profiles.Domain.Model.Entities;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class ClientGymAssociationResourceFromEntityAssembler
{
    public static ClientGymAssociationResource ToResourceFromEntity(ClientGymAssociation association) =>
        new(association.ClientId, association.GymId, association.Active);
}
