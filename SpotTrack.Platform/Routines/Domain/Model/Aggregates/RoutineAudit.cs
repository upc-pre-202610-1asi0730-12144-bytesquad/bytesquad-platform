using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Routines.Domain.Model.Aggregates;

public partial class Routine : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}