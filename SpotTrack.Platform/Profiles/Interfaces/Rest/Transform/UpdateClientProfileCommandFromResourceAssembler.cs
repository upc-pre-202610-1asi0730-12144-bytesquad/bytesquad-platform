using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class UpdateClientProfileCommandFromResourceAssembler
{
    public static UpdateClientProfileCommand ToCommandFromResource(int clientId, UpdateClientProfileResource resource) =>
        new(clientId, resource.FirstName, resource.LastName, resource.PhoneNumber);
}
