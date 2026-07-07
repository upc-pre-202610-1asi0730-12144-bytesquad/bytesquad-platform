using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class ChangeActiveGymCommandFromResourceAssembler
{
    public static ChangeActiveGymCommand ToCommandFromResource(int clientId, ChangeActiveGymResource resource)
        => new(clientId, resource.GymId);
}
