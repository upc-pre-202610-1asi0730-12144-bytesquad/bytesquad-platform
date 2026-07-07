using SpotTrack.Platform.Iam.Domain.Model.Commands;
using SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Iam.Interfaces.Rest.Transform;

public static class ChangePasswordCommandFromResourceAssembler
{
    public static ChangePasswordCommand ToCommandFromResource(int userId, ChangePasswordResource resource) =>
        new(userId, resource.CurrentPassword, resource.NewPassword);
}
