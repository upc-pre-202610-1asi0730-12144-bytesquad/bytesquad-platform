using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Maintenances.Domain.Model.Aggregates;

public partial class MaintenanceJob : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
