using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Analytics.Domain.Model.Aggregates;

public partial class ROIProjection : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
