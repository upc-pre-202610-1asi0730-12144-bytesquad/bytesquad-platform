using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user) =>
        new(user.Id, user.Username, user.Role.ToString());
}
