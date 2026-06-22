using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class CreateAdminCommandFromResourceAssembler
{
    public static CreateAdminCommand ToCommandFromResource(CreateAdminResource resource) =>
        new(resource.UserId, resource.Email, resource.FirstName, resource.LastName,
            resource.PhoneNumber, resource.Dni);
}
