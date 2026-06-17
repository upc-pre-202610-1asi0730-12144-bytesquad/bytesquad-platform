using SpotTrack.Platform.Routines.Domain.Model.Aggregates;
using SpotTrack.Platform.Routines.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Routines.Interfaces.Rest.Transform;

public static class RoutineSessionResourceFromEntityAssembler
{
    public static RoutineSessionResource ToResourceFromEntity(RoutineSession session) =>
        new(session.Id, session.RoutineId, session.ClientId.Value, session.Status.ToString(), session.StartedAt);
}
