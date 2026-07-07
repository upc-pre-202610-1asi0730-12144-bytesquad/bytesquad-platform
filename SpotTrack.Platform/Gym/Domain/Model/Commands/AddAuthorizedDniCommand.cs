namespace SpotTrack.Platform.Gyms.Domain.Model.Commands;

public record AddAuthorizedDniCommand(int GymId, string Dni);
