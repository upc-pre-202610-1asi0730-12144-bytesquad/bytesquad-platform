using SpotTrack.Platform.Shared.Domain.Model.Events;

namespace SpotTrack.Platform.Gyms.Domain.Model.Events;

public record GymDniRevokedEvent(int GymId, string Dni) : IEvent;
