using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Routines.Interfaces.Rest.Transform;

public static class CreateRoutineCommandFromResourceAssembler
{
    public static CreateRoutineCommand ToCommandFromResource(CreateRoutineResource resource) =>
        new(resource.ClientId, resource.RoutineName);
}
