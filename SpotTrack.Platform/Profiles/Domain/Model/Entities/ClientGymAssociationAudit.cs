using SpotTrack.Platform.Shared.Domain.Model.Entities;

namespace SpotTrack.Platform.Profiles.Domain.Model.Entities;

public partial class ClientGymAssociation : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
