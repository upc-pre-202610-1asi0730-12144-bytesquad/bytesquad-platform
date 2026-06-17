using SpotTrack.Platform.Profiles.Domain.Model.Aggregates;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class AdminResourceFromEntityAssembler
{
    public static AdminResource ToResourceFromEntity(Admin admin) =>
        new(admin.Id, admin.UserId, admin.FullName, admin.Email?.Address ?? string.Empty,
            admin.Phone?.Number ?? string.Empty, admin.Dni?.Value ?? string.Empty);
}
