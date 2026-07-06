namespace SpotTrack.Platform.Iam.Domain.Model.Commands;

public record ProvisionIamAccountCommand(string Email, string AlreadyHashedPassword);
