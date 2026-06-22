namespace SpotTrack.Platform.Maintenances.Interfaces.Rest.Resources;

public record RegisterMaintenanceCompletionResource(int TechnicalTicketId, int CompletedByAdminId, string Notes);
