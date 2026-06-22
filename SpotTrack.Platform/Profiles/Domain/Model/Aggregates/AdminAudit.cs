using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Profiles.Domain.Model.Aggregates;

public partial class Admin : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
