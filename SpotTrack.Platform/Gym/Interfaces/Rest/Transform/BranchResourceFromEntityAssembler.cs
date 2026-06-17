using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class BranchResourceFromEntityAssembler
{
    public static BranchResource ToResourceFromEntity(Branch branch) =>
        new(branch.Id, branch.Name.Value, branch.Address.Street, branch.Address.District, branch.Address.City);
}
