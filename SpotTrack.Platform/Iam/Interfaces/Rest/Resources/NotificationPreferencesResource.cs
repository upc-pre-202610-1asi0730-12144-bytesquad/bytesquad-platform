namespace SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

public record NotificationPreferencesResource(bool NotifyOnCritical, bool NotifyOnWarning, string? NotificationEmail);
