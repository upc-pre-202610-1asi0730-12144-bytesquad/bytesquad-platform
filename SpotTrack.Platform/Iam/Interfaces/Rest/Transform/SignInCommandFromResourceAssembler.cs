using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Iam.Interfaces.Rest.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource) =>
        new(resource.Username, resource.Password);
}
