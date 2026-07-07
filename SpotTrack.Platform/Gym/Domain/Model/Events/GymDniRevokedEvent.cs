using Cortex.Mediator.Notifications;

namespace SpotTrack.Platform.Gyms.Domain.Model.Events;

public record GymDniRevokedEvent(int GymId, string Dni) : INotification;
