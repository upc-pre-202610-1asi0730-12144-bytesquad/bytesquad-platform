using SpotTrack.Platform.Gyms.Domain.Model.Commands;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class CreateBranchCommandFromResourceAssembler
{
    public static CreateBranchCommand ToCommandFromResource(int gymId, CreateBranchResource resource) =>
        new(gymId, resource.Name, resource.Street, resource.District, resource.City);
}
