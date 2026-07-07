namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record RemoveAuthorizedDniCommand(int GymId, string Dni);
