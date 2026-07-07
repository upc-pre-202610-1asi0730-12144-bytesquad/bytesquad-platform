namespace SpotTrack.Platform.Iam.Domain.Model.Commands;

public record VerifyForgotPasswordCommand(string Username, string Code, string NewPassword);
