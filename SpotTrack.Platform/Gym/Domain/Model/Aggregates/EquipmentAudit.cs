using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Gyms.Domain.Model.Aggregates;

public partial class Equipment : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
