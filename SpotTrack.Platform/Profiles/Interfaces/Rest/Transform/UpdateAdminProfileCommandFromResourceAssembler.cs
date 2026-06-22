using SpotTrack.Platform.Profiles.Domain.Model.Commands;
using SpotTrack.Platform.Profiles.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Profiles.Interfaces.Rest.Transform;

public static class UpdateAdminProfileCommandFromResourceAssembler
{
    public static UpdateAdminProfileCommand ToCommandFromResource(int adminId, UpdateAdminProfileResource resource) =>
        new(adminId, resource.FirstName, resource.LastName, resource.PhoneNumber);
}
