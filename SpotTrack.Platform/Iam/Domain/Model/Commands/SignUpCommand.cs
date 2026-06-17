namespace SpotTrack.Platform.Iam.Domain.Model.Commands;

public record SignUpCommand(string Username, string Password, string Role);
