using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class ClientResourceFromEntityAssembler
{
    public static ClientResource ToResourceFromEntity(Client client) =>
        new(client.Id, client.UserId, client.FullName, client.Email?.Address ?? string.Empty,
            client.Phone?.Number ?? string.Empty, client.Dni?.Value ?? string.Empty);
}
