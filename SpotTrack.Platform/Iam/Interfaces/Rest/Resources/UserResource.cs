namespace SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

public record UserResource(
    int Id,
    string Username,
    string Role,
    bool NotifyOnCritical,
    bool NotifyOnWarning,
    string? NotificationEmail);
