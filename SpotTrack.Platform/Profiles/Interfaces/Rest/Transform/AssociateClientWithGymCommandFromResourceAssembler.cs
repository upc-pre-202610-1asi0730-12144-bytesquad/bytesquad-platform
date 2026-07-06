using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class AssociateClientWithGymCommandFromResourceAssembler
{
    public static AssociateClientWithGymCommand ToCommandFromResource(int clientId, AssociateGymResource resource) =>
        new(clientId, resource.GymId);
}
