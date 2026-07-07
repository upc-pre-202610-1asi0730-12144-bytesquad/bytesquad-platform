namespace SpotTrack.Platform.Iam.Domain.Model.Commands;

public record UpdateNotificationPreferencesCommand(
    int UserId,
    bool NotifyOnCritical,
    bool NotifyOnWarning,
    string? NotificationEmail);
