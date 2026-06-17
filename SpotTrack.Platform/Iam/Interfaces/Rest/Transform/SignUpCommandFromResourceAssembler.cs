using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Iam.Interfaces.Rest.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource) =>
        new(resource.Username, resource.Password, resource.Role);
}
