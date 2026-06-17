using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Routines.Domain.Model.Aggregates;

public partial class RoutineSession : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
