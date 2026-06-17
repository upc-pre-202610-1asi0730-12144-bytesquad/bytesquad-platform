using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Memberships.Domain.Model.Aggregates;

public partial class Membership : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
