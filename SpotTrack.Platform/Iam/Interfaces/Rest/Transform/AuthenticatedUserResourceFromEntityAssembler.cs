using SpotTrack.Platform.Iam.Domain.Model.Aggregates;
using SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Iam.Interfaces.Rest.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token) =>
        new(user.Id, user.Username, user.Role.ToString(), token);
}
