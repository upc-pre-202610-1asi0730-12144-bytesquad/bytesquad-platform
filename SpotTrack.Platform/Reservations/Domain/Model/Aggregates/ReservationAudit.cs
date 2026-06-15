using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Reservations.Domain.Model.Aggregates;

public partial class Reservation : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
