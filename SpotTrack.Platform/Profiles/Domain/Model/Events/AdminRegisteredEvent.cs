using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Profiles.Domain.Model.Events;

public record AdminRegisteredEvent(
    int AdminId,
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Dni) : IEvent
{
    public static AdminRegisteredEvent FromAdmin(Admin admin) =>
        new(admin.Id,
            admin.UserId,
            admin.Name!.FirstName,
            admin.Name!.LastName,
            admin.Email!.Address,
            admin.Phone!.Number,
            admin.Dni!.Value);
}
