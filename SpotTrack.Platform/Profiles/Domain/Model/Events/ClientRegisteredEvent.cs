using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Profiles.Domain.Model.Events;

public record ClientRegisteredEvent(
    int ClientId,
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Dni) : IEvent
{
    public static ClientRegisteredEvent FromClient(Client client) =>
        new(client.Id,
            client.UserId,
            client.Name!.FirstName,
            client.Name!.LastName,
            client.Email!.Address,
            client.Phone!.Number,
            client.Dni!.Value);
}
