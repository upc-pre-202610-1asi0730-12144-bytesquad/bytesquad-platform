using SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;
using SpotTrack.Platform.Monitoring.Interfaces.Rest.Resources;

namespace SpotTrack.Platform.Monitoring.Interfaces.Rest.Transform;

public static class SessionTrackerResourceFromEntityAssembler
{
    public static SessionTrackerResource ToResourceFromEntity(SessionTracker tracker) =>
        new(tracker.Id, tracker.EquipmentId, tracker.AdminId, tracker.StartedAt, tracker.EndedAt, tracker.IsActive);

    public static SessionTrackerTimeResource ToTimeResourceFromEntity(SessionTracker tracker) =>
        new(tracker.Id, tracker.IsActive, tracker.ElapsedSeconds);
}
