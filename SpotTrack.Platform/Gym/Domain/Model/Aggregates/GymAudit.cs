using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Gym.Domain.Model.Aggregates;

public partial class Gym : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
