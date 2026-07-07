namespace SpotTrack.Platform.Iam.Interfaces.Rest.Resources;

public record VerifyForgotPasswordResource(string Username, string Code, string NewPassword);
