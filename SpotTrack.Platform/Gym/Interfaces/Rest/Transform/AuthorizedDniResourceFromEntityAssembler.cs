using SpotTrack.Platform.Gyms.Domain.Model.Entities;
using SpotTrack.Platform.Gyms.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Gyms.Interfaces.Rest.Transform;

public static class AuthorizedDniResourceFromEntityAssembler
{
    public static AuthorizedDniResource ToResourceFromEntity(AuthorizedDni entity)
        => new(entity.Id, entity.GymId, entity.Dni);
}
