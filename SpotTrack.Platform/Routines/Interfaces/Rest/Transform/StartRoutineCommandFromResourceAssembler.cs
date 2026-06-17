using SpotTrack.Platform.Routines.Domain.Model.Commands;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Routines.Interfaces.Rest.Transform;

public static class StartRoutineCommandFromResourceAssembler
{
    public static StartRoutineCommand ToCommandFromResource(StartRoutineResource resource) =>
        new(resource.RoutineId, resource.ClientId);
}
