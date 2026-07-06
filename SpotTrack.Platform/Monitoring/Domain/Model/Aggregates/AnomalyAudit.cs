using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Monitoring.Domain.Model.Aggregates;

public partial class Anomaly : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
