namespace SpotTrack.Platform.Maintenances.Domain.Model.Commands;

public record ModifyTicketStatusCommand(int TechnicalTicketId, ETechnicalTicketStatus NewStatus);
